using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{
    public class AEVE_Properties : ActiveElementTreeViewBox
    {
        public override void BindItem(HierarchyData item)
        {
            activeElement = item;

            SetupFields();
        }

        private void SetupFields()
        {


        }


        public override void OnGUI()
        {
            base.OnGUI();
        }





    }
}
