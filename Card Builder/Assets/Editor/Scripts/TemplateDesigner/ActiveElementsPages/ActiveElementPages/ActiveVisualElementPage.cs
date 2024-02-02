using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    public class ActiveVisualElementPage : ElementPage<ActiveElementTreeViewBox, HierarchyData>
    {
        public override void Initialize(VisualElement visualElement)
        {
            base.Initialize(visualElement);

            AddBox<AEVE_Main>();
            AddBox<AEVE_Transform>();         
        }
    }
}
