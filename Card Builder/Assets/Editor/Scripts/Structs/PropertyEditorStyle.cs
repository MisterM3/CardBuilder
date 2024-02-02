using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{
    [Serializable]
    public struct PropertyEditorStyle 
    {
        public Color backgroundColour;
        public Color textColour;
        public PropertyEditorStyle(Color backgroundColour, Color textColour)
        {
            this.backgroundColour = backgroundColour;
            this.textColour = textColour;
        }
    }
}
