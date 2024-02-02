using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    /// <summary>
    /// Generic Page for active element panels, every elementpage has a different property or treeviewitem that it corresponds to
    /// </summary>
    /// <typeparam name="T">ActiveElementBox which the list consists of</typeparam>
    /// <typeparam name="U">Data type (HierarchyData, or PropertyInfo)</typeparam>
    public class ElementPage<T, U> : ScriptableObject where T : ActiveElementBox<U>
    {
        protected VisualElement m_viewWindow;

        protected List<T> propertyBoxes = new();

        public virtual void Initialize(VisualElement visualElement) 
        {
            m_viewWindow = visualElement;

            //Keeps for safety, maybe remove
            foreach (T propertyBox in propertyBoxes)
            {
                propertyBox.Initialize(m_viewWindow);
            }
        }


        #region Adding

        //Adds a box to the panel (Doesn't work for reason, try to fix later)
        protected void AddBox<TpropertyBox>() where TpropertyBox : T
        {
            TpropertyBox boxToAdd = CreateInstance<TpropertyBox>();
            boxToAdd.Initialize(m_viewWindow);
            propertyBoxes.Add(boxToAdd);
        }

        #endregion

        #region VirtualMethods

        #region GUI




        public virtual void CreateGUI()
        {
            foreach (T propertyBox in propertyBoxes)
            {
                propertyBox.CreateGUI();
            }
        }

        public virtual void RemoveGUI()
        {
            foreach (T propertyBox in propertyBoxes)
            {
                propertyBox.Remove();
            }
        }
        public virtual void OnGUI() 
        {
            foreach (T propertyBox in propertyBoxes)
            {
                propertyBox.OnGUI();
            }
        }

        #endregion
        public virtual void BindItem(U item) 
        {
            foreach (T propertyBox in propertyBoxes)
            {
                propertyBox.BindItem(item);

            }
        }

        #endregion


    }
}
