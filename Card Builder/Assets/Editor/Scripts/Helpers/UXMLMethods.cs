namespace CardBuilder.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.UIElements;


    public static class UXMLMethods
    {
        
        public static void ConvertToSplitView(this VisualElement currentSplit, TwoPaneSplitViewOrientation orientation = TwoPaneSplitViewOrientation.Horizontal)
        {
            int size = 350;

            TwoPaneSplitView splitView = new TwoPaneSplitView(0, size, orientation);

            List<VisualElement> children = currentSplit.Children().ToList();

            for (int i = children.Count - 1; i >= 0; i--) splitView.Insert(0, children[i]);

            VisualElement parentSplit = currentSplit.parent;

            parentSplit.Remove(currentSplit);
            parentSplit.Add(splitView);

            parentSplit.style.flexGrow = 1;
            splitView.style.flexGrow = 1;
        }
        /// <summary>
        /// Works like Q, but gives a debug.warning if it didn't find anything
        /// </summary>
        /// <typeparam name="TVisualElement"></typeparam>
        /// <param name="visualElement"></param>
        /// <param name="nameElementToFind"></param>
        /// <returns></returns>
        public static TVisualElement QLogged<TVisualElement>(this VisualElement visualElement, string nameElementToFind = null) where TVisualElement : VisualElement
        {
            TVisualElement searchedVisualElement = visualElement.Q<TVisualElement>(nameElementToFind);

            if (searchedVisualElement == null)
            {
                Logs.Warning("VisualElement not found using Q, maybe a typo?" + visualElement);
                return null;
            }

            return searchedVisualElement;
        }
    }
}
