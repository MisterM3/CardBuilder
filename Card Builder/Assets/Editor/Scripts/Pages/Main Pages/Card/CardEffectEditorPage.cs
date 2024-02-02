using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CardBuilder
{

    using UI;
    using GraphView;
    using UnityEditor;
    using UnityEngine.UIElements;
    using UnityEditor.Experimental.GraphView;
    using System.Linq;
    using CardBuilder.Helpers;

    public class CardEffectEditorPage : PageSO
    {
        
        public override void Initialize(CardBuilderEditor editor)
        {
            base.Initialize(editor);
        }

        protected override void RegisterEvents()
        {
            CreateSplitView(m_VisualElement);

            Button button = m_VisualElement.QLogged<Button>("MenuButton");


            button.clicked += () =>
            {
                int option = EditorUtility.DisplayDialogComplex("Unsaved Progress", "Do you want to save the card before going to the menu?", "Save", "Discard", "Cancel");

                switch (option)
                {
                    case 0:
                        Logs.Info("Save File");
                        m_editor.SwitchPage(EPages.StartingPage);
                        break;
                    case 1:
                        m_editor.SwitchPage(EPages.StartingPage);
                        break;
                    case 2:
                        break;
                }

            };

            Button buttonGoToEffect = m_VisualElement.QLogged<Button>("GoPropertyButton");


            buttonGoToEffect.clicked += () =>
            {
                int option = EditorUtility.DisplayDialogComplex("Unsaved Progress", "Do you want to save the card before going to the property page?", "Save", "Discard", "Cancel");

                switch (option)
                {
                    case 0:
                        Logs.Info("Save File");
                        m_editor.SwitchPage(EPages.CardPropertyEditorPage);
                        break;
                    case 1:
                        m_editor.SwitchPage(EPages.CardPropertyEditorPage);
                        break;
                    case 2:
                        break;
                }


            };



            CreateGraphViewGUI(m_VisualElement);

        }

        #region SplitView

        private void CreateSplitView(VisualElement visualTree)
        {
            CreateGraphViewPropertySlitView(visualTree);

            CreatePropertySplitView(visualTree);
        }

        private void CreateGraphViewPropertySlitView(VisualElement visualTree)
        {
            VisualElement twoPanePropertyGraphView = visualTree.QLogged<VisualElement>("TwoPaneReplace");

            TwoPaneSplitView splitView = new TwoPaneSplitView(0, 350, TwoPaneSplitViewOrientation.Horizontal);

            List<VisualElement> children = twoPanePropertyGraphView.Children().ToList();

            for (int i = children.Count - 1; i >= 0; i--) splitView.Insert(0, children[i]);


            VisualElement parentSplit = twoPanePropertyGraphView.parent;

            parentSplit.Remove(twoPanePropertyGraphView);
            parentSplit.Add(splitView);

            parentSplit.style.flexGrow = 1;
            splitView.StretchToParentSize();
        }

        private void CreatePropertySplitView(VisualElement visualTree)
        {
            VisualElement twoPanePropertyGraphView = visualTree.QLogged<VisualElement>("PreviewTwoPane");

            TwoPaneSplitView splitView = new TwoPaneSplitView(0, 100, TwoPaneSplitViewOrientation.Vertical);

            List<VisualElement> children = twoPanePropertyGraphView.Children().ToList();

            for (int i = children.Count - 1; i >= 0; i--) splitView.Insert(0, children[i]);


            VisualElement parentSplit = twoPanePropertyGraphView.parent;

            parentSplit.Remove(twoPanePropertyGraphView);
            parentSplit.Add(splitView);

            parentSplit.style.flexGrow = 1;
            splitView.style.flexGrow = 1;
        }


        #endregion

        private void CreateGraphViewGUI(VisualElement visualTree)
        {
            VisualElement graphSide = visualTree.QLogged<VisualElement>("NodeGraph");

            CB_GraphView graphView = new CB_GraphView();

            graphSide.Add(graphView);

            graphView.StretchToParentSize();
        }

        public override void SetupUI()
        {
            throw new System.NotImplementedException();
        }

        protected override void UnRegisterEvents()
        {
            throw new System.NotImplementedException();
        }
    }
}
