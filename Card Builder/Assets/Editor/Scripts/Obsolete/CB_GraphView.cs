
namespace CardBuilder.GraphView
{
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;

    public class CB_GraphView : GraphView
    {
        public CB_GraphView()
        {
            AddGridBackground();
        }



        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
    }


}
