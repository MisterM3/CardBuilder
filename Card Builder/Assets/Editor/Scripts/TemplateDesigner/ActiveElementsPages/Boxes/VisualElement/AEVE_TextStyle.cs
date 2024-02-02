using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using System;
using CardBuilder.Helpers;
using TMPro;
namespace CardBuilder
{
    public class AEVE_TextStyle : ActiveElementTreeViewBox
    {
        EnumField horizontalEnumField;
        EnumField verticalEnumField;

        FloatField fontFloatField;

        ObjectField fontField;

        ColorField colourField;

        [SerializeField]
        private TMP_FontAsset m_defaultFont = default;

        public override void BindItem(HierarchyData item)
        {
            activeElement = item;

            if (activeElement.VisualElement is not UndoRedoText undoRedoText)
            {
                Logs.Error("Not supported element");
                return;
            }

            horizontalEnumField.value = undoRedoText.HorAlign;
            verticalEnumField.value = undoRedoText.VerAlign;

            fontFloatField.value = undoRedoText.FontSize;
            colourField.value = undoRedoText.TextColour;

            if (undoRedoText.Font == null)
                undoRedoText.Font = m_defaultFont;

            fontField.value = undoRedoText.Font;

        }

        #region Setup
        public override void SetupUI()
        {
            SetupFields();
        }

        private void SetupFields()
        {
            SetupFont();
            SetupColour();
            SetupEnums();
        }

        private void SetupFont()
        {
            fontField = m_VisualElement.QLogged<ObjectField>("FontField");
            fontFloatField = m_VisualElement.QLogged<FloatField>("FontSize");
        }

        private void SetupColour()
        {
            colourField = m_VisualElement.QLogged<ColorField>("TextColour");
        }

        private void SetupEnums()
        {
            horizontalEnumField = m_VisualElement.QLogged<EnumField>("HorAlign");

            verticalEnumField = m_VisualElement.QLogged<EnumField>("VerAlign");
        }

        #endregion

        #region RegisterEvents

        protected override void RegisterEvents()
        {
            horizontalEnumField.RegisterValueChangedCallback(OnHorizontalFieldChange);
            verticalEnumField.RegisterValueChangedCallback(OnVerticalFieldChange);
            fontFloatField.RegisterCallback<KeyDownEvent>(ChangeSize);
            fontField.RegisterValueChangedCallback(ChangeFont);
            colourField.RegisterValueChangedCallback(OnColourFieldChange);
        }

        #endregion

        #region UnRegisterEvents

        protected override void UnRegisterEvents()
        {
            horizontalEnumField.UnregisterValueChangedCallback(OnHorizontalFieldChange);
            verticalEnumField.UnregisterValueChangedCallback(OnVerticalFieldChange);
            fontFloatField.UnregisterCallback<KeyDownEvent>(ChangeSize);
            fontField.UnregisterValueChangedCallback(ChangeFont);
            colourField.UnregisterValueChangedCallback(OnColourFieldChange);
        }

        #endregion

        #region Events

        private void OnHorizontalFieldChange(ChangeEvent<Enum> evt)
        {
            if (activeElement == null)
            {
                Logs.NoActiveElementError();
                return;
            }


            if (activeElement.VisualElement is not UndoRedoText undoRedoText)
            {
                Logs.Error("Not supported element");
                return;
            }
            undoRedoText.HorAlign = (EAlign)evt.newValue;
        }

        private void OnVerticalFieldChange(ChangeEvent<Enum> evt)
        {
            if (activeElement == null)
            {
                Logs.NoActiveElementError();
                return;
            }


            if (activeElement.VisualElement is not UndoRedoText undoRedoText)
            {
                Logs.Error("Not supported element");
                return;
            }

            undoRedoText.VerAlign = (EAlignHor)evt.newValue;

        }

        private void OnColourFieldChange(ChangeEvent<Color> evt)
        {
            if (activeElement == null)
            {
                Logs.NoActiveElementError();
                return;
            }


            if (activeElement.VisualElement is not UndoRedoText undoRedoText)
            {
                Logs.Error("Not supported element");
                return;
            }

            undoRedoText.TextColour = evt.newValue;

        }



        private void ChangeSize(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return)
            {
                if (activeElement.VisualElement is not UndoRedoText undeRedoText)
                {
                    Logs.Error("Not supported element");
                    return;
                }

                
                undeRedoText.FontSize = fontFloatField.value;
            }
        }

        private void ChangeFont(ChangeEvent<UnityEngine.Object> evt)
        {
            if (activeElement.VisualElement is not UndoRedoText undeRedoText)
            {
                Logs.Error("Not supported element");
                return;
            }

            undeRedoText.Font = (TMP_FontAsset)evt.newValue;
        }

        #endregion

    }
}
