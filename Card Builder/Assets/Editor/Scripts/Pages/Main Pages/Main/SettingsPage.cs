using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{
    using UI;
    using UnityEditor;
    using UnityEngine.UIElements;
    using CardBuilder.Helpers;
    public class SettingsPage : PageSO
    {



        protected override void RegisterEvents()
        {

            Button button = m_VisualElement.QLogged<Button>("Save");

            button.clicked += SaveSettings;

            LoadSettings();
        }


        public void SaveSettings()
        {

        }


        public void LoadSettings()
        {


            

        }

        public override void SetupUI()
        {

        }

        protected override void UnRegisterEvents()
        {

        }
    }
}
