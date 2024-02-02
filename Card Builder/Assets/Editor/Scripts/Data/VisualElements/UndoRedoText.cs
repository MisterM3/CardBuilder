using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    [Serializable]
    public class UndoRedoText : UndoRedoVisualElement
    {
        [SerializeField] private List<string> textList;

        public string Text
        {
            get
            {
                if (textList == null) textList = new();
                if (textList.Count == 0) textList.Add("");
                return textList[0];
            }

            set
            {
                if (textList.Count == 0) textList.Add(value);
                else textList[0] = value;

                Label field = (Label)VisualElement;
                field.text = textList[0];
            }

        }

        public List<String> TextList
        {
            get => textList;

            set => textList = value;
        }


        [SerializeField] TMP_FontAsset font;

        public TMP_FontAsset Font
        {
            get
            {
                return font;
            }
            set
            {
                Label field = (Label)VisualElement;
                
                field.style.unityFontDefinition = new StyleFontDefinition(value.sourceFontFile);
                field.style.unityFont = value.sourceFontFile;
                font = value;
            }
        }


        [SerializeField] private float fontSize = 12;

        public float FontSize
        {
            get
            {                    
                return fontSize;
            }

            set
            {
                Label field = (Label)VisualElement;
                field.style.fontSize = new StyleLength(value);
                fontSize = value;
            }
        }


        [SerializeField] Color textColour = Color.black;

        public Color TextColour
        {
            get => textColour;

            set
            {
                VisualElement.style.color = new StyleColor(value);
                textColour = value;
            }
        }


        //Cursed code for transforming enums


        [SerializeField] TextAnchor textAnchor = TextAnchor.UpperLeft;

        public EAlign HorAlign
        {
            get
            {
                TextAnchor anchor = textAnchor;
                switch (anchor)
                {
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.UpperLeft:
                    case TextAnchor.LowerLeft:
                        return EAlign.Left;

                    case TextAnchor.LowerCenter:
                    case TextAnchor.UpperCenter:
                    case TextAnchor.MiddleCenter:
                        return EAlign.Center;

                    case TextAnchor.LowerRight:
                    case TextAnchor.MiddleRight:
                    case TextAnchor.UpperRight:
                        return EAlign.Right;

                    default:
                        break;
                }

                return EAlign.Left;
            }

            set
            {
                switch (VerAlign)
                {
                    case EAlignHor.Upper:

                        switch (value)
                        {
                            case EAlign.Left:
                                VisualElement.style.unityTextAlign = TextAnchor.UpperLeft;
                                textAnchor = TextAnchor.UpperLeft;
                                break;
                            case EAlign.Center:
                                VisualElement.style.unityTextAlign = TextAnchor.UpperCenter;
                                textAnchor = TextAnchor.UpperCenter;
                                break;
                            case EAlign.Right:
                                VisualElement.style.unityTextAlign = TextAnchor.UpperRight;
                                textAnchor = TextAnchor.UpperRight;
                                break;
                        }
                        break;

                    case EAlignHor.Middle:

                        switch (value)
                        {
                            case EAlign.Left:
                                VisualElement.style.unityTextAlign = TextAnchor.MiddleLeft;
                                textAnchor = TextAnchor.MiddleLeft;
                                break;
                            case EAlign.Center:
                                VisualElement.style.unityTextAlign = TextAnchor.MiddleCenter;
                                textAnchor = TextAnchor.MiddleCenter;
                                break;
                            case EAlign.Right:
                                VisualElement.style.unityTextAlign = TextAnchor.MiddleRight;
                                textAnchor = TextAnchor.MiddleRight;
                                break;
                        }
                        break;

                    case EAlignHor.Lower:

                        switch (value)
                        {
                            case EAlign.Left:
                                VisualElement.style.unityTextAlign = TextAnchor.LowerLeft;
                                textAnchor = TextAnchor.LowerLeft;
                                break;
                            case EAlign.Center:
                                VisualElement.style.unityTextAlign = TextAnchor.LowerCenter;
                                textAnchor = TextAnchor.LowerCenter;
                                break;
                            case EAlign.Right:
                                VisualElement.style.unityTextAlign = TextAnchor.LowerRight;
                                textAnchor = TextAnchor.LowerRight;
                                break;
                        }
                        break;
                }
            }
        }

        public EAlignHor VerAlign
        {
            get
            {
                TextAnchor anchor = textAnchor;
                switch (anchor)
                {
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.MiddleRight:
                        return EAlignHor.Middle;

                    case TextAnchor.UpperLeft:
                    case TextAnchor.UpperCenter:
                    case TextAnchor.UpperRight:
                        return EAlignHor.Upper;

                    case TextAnchor.LowerLeft:
                    case TextAnchor.LowerCenter:
                    case TextAnchor.LowerRight:

                    default:
                        break;
                }

                return EAlignHor.Lower;
            }

            set
            {
                switch(HorAlign)
                {
                    case EAlign.Left:
                        
                        switch(value)
                        {
                            case EAlignHor.Lower:
                                VisualElement.style.unityTextAlign = TextAnchor.LowerLeft;
                                textAnchor = TextAnchor.LowerLeft;
                                break;
                            case EAlignHor.Middle:
                                VisualElement.style.unityTextAlign = TextAnchor.MiddleLeft;
                                textAnchor = TextAnchor.MiddleLeft;
                                break;
                            case EAlignHor.Upper:
                                VisualElement.style.unityTextAlign = TextAnchor.UpperLeft;
                                textAnchor = TextAnchor.UpperLeft;
                                break;
                        }
                     break;

                    case EAlign.Center:

                        switch (value)
                        {
                            case EAlignHor.Lower:
                                VisualElement.style.unityTextAlign = TextAnchor.LowerCenter;
                                textAnchor = TextAnchor.LowerCenter;
                                break;
                            case EAlignHor.Middle:
                                VisualElement.style.unityTextAlign = TextAnchor.MiddleCenter;
                                textAnchor = TextAnchor.MiddleCenter;
                                break;
                            case EAlignHor.Upper:
                                VisualElement.style.unityTextAlign = TextAnchor.UpperCenter;
                                textAnchor = TextAnchor.UpperCenter;
                                break;
                        }
                    break;

                    case EAlign.Right:

                        switch (value)
                        {
                            case EAlignHor.Lower:
                                VisualElement.style.unityTextAlign = TextAnchor.LowerRight;
                                textAnchor = TextAnchor.LowerRight;
                                break;
                            case EAlignHor.Middle:
                                VisualElement.style.unityTextAlign = TextAnchor.MiddleCenter;
                                textAnchor = TextAnchor.MiddleCenter;
                                break;
                            case EAlignHor.Upper:
                                VisualElement.style.unityTextAlign = TextAnchor.UpperRight;
                                textAnchor = TextAnchor.UpperRight;
                                break;
                        }
                        break;
                }
            }
        }

        public TextAlignmentOptions TMP_Alignment
        {
            get
            {
                switch (HorAlign)
                {
                    case EAlign.Left:

                        switch (VerAlign)
                        {
                            case EAlignHor.Lower:
                                return TextAlignmentOptions.BottomLeft;

                            case EAlignHor.Middle:
                                return TextAlignmentOptions.Left;

                            case EAlignHor.Upper:
                                return TextAlignmentOptions.TopLeft;

                        }
                        break;

                    case EAlign.Center:

                        switch (VerAlign)
                        {
                            case EAlignHor.Lower:
                                return TextAlignmentOptions.Bottom;

                            case EAlignHor.Middle:
                                return TextAlignmentOptions.Center;

                            case EAlignHor.Upper:
                                return TextAlignmentOptions.Top;

                        }
                        break;

                    case EAlign.Right:

                        switch (VerAlign)
                        {
                            case EAlignHor.Lower:
                                return TextAlignmentOptions.BottomRight;

                            case EAlignHor.Middle:
                                return TextAlignmentOptions.Right;

                            case EAlignHor.Upper:
                                return TextAlignmentOptions.TopRight;

                        }
                        break;
                }
                return TextAlignmentOptions.Midline;
            }

        }

        public new void Init()
        {


            this.visualElement = new Label(Text);
            this.visualElement.style.fontSize = new StyleLength(fontSize);
            this.visualElement.style.color = textColour;



            if (font != null)
            {
                Label field = (Label)VisualElement;
                field.style.unityFontDefinition = new StyleFontDefinition(font.sourceFontFile);
                field.style.unityFont = font.sourceFontFile;
            }

            this.visualElement.style.unityTextAlign = textAnchor;

            this.visualElement.style.whiteSpace = WhiteSpace.Normal;
            
            this.VisualElement.style.alignSelf = Align.Center;
            this.visualElement.style.width = Size.x;
            this.visualElement.style.height = Size.y;

            Position = Position;
           
            this.visualElement.style.position = UnityEngine.UIElements.Position.Absolute;


            UpdateMovement();

        }

        public UndoRedoText Copy()
        {
            return new UndoRedoText
            {
                nameObject = this.nameObject,
                Position = this.Position,
                Size = this.Size,
                ConnectedInfo = this.ConnectedInfo.Copy(),
                Text = this.Text,
                TextColour = this.TextColour,
                textAnchor = this.textAnchor,
                Font = this.Font,
                FontSize = this.FontSize
            };
        }
    }
}
