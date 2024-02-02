using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    public class ActiveTextVisualElementPage : ActiveVisualElementPage
    {
        public override void Initialize(VisualElement visualElement)
        {
            base.Initialize(visualElement);

            AddBox<AEVE_Text>();
            AddBox<AEVE_TextStyle>();
        }
    }
}
