namespace CardBuilder
{
    using UnityEngine;
    using UnityEngine.UIElements;
    using CardBuilder.Helpers;
    /// <summary>
    /// Box for changing the position and size of a visualelement (For Card Template)
    /// </summary>
    public class AEVE_Transform : ActiveElementTreeViewBox
    {

        bool lastChangeGizmo = false;

        RectField transformRectField;



        #region Binding

        public override void BindItem(HierarchyData item)
        {

            if (activeElement != null) activeElement.VisualElement.OnRectChange -= ChangeRectField;

            base.BindItem(item);

            activeElement.VisualElement.OnRectChange += ChangeRectField;

            BindRectField();
        }

        private void BindRectField()
        {

            ChangeRectField(this, new Rect(activeElement.Position, activeElement.Size));
        }

        #endregion

        #region Setup

        public override void SetupUI()
        {
            SetupFields();
        }
        private void SetupFields()
        {
            SetupRectField();
        }
        private void SetupRectField()
        {
            transformRectField = m_VisualElement.QLogged<RectField>("TransformField");

            transformRectField.RegisterValueChangedCallback(RectFieldChange);

        }


        #endregion

        #region RegisterEvents

        protected override void RegisterEvents()
        {
            transformRectField.RegisterValueChangedCallback(RectFieldChange);
        }


        #endregion

        #region UnRegisterEvents
        protected override void UnRegisterEvents()
        {
            transformRectField.UnregisterValueChangedCallback(RectFieldChange);

            if (activeElement!= null) activeElement.VisualElement.OnRectChange -= ChangeRectField;
        }

        #endregion

        #region Events


        private void ChangeRectField(object sender, Rect rect)
        {


            if (transformRectField.value != new Rect(rect.x, rect.y, rect.width, rect.height))
            {
                
                lastChangeGizmo = true;
                transformRectField.value = new Rect(rect.x, rect.y, rect.width, rect.height);
            }

        }
        private void RectFieldChange(ChangeEvent<Rect> evt)
        {

            if (lastChangeGizmo)
            {
                lastChangeGizmo = false;
                return;
            }
            activeElement.Position = new Vector2(evt.newValue.x, evt.newValue.y);
            activeElement.Size = new Vector2(evt.newValue.width, evt.newValue.height);
        }

        #endregion


    }
}
