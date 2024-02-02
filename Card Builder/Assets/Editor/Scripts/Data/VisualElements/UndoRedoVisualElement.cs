using CardBuilder;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class UndoRedoVisualElement
{
    public string nameObject;

    [SerializeField] protected VisualElement visualElement;

    public VisualElement VisualElement
    {
        get =>  visualElement; 
    }
    [SerializeField] private Vector2 position = Vector2.zero;

    public Vector2 Position
    {
        set
        {
            position = (value);
            OnRectChange?.Invoke(this, new Rect(position, Size));
            UpdateMovement();
        }

        get => position;
    }

    [SerializeField] private PropertyInfo connectedInfo;

    public PropertyInfo ConnectedInfo { get => connectedInfo; set => connectedInfo = value; }
    



    [SerializeField] private Vector2 size = new Vector2(100, 100);

    public Vector2 Size
    {
        set
        {
            size = value;
            Logs.Info("eva");
            OnRectChange?.Invoke(this, new Rect(Position, value));
            UpdateSize();
        }

        get => size;
    }

    public event EventHandler<Rect> OnRectChange;


    public void Init(VisualElement element)
    {
        this.visualElement = element;
    }

    //Initialize visualElement itself
    public void Init()
    {
        this.visualElement = new();
        this.VisualElement.style.alignSelf = Align.Center;

        
        this.visualElement.style.width = Size.x;
        this.visualElement.style.height = Size.y;

        //Testing
    //    this.visualElement.style.backgroundColor = Color.gray;
        this.visualElement.style.position = UnityEngine.UIElements.Position.Absolute;

        UpdateMovement();
    }


    public void OnEnable()
    {
        visualElement = new VisualElement();

        visualElement.style.width = 50;
        visualElement.style.height = 50;
        size = new Vector2(50, 50);
        visualElement.style.backgroundColor = Color.red;
    }

    public void MoveVisualElement(Vector2 moveAmount)
    {
      //  Undo.RegisterCompleteObjectUndo(this, "Move Image");
        Position += moveAmount;
        UpdateMovement();
     //   EditorUtility.SetDirty(this);
    }

    public void RemoveVisualElement()
    {
        if (this.visualElement.parent != null)
        this.visualElement.parent.Remove(this.visualElement);
    }

    public bool ScaleVisualElement(Vector2 scaleAmount)
    {
        bool underZero = false;

     //   Undo.RegisterCompleteObjectUndo(this, "Scale Image");
        
        Size += scaleAmount;

        if (Size.x <= 0 || Size.y <= 0) underZero = true;
        size.x = Mathf.Max(0, Size.x);
        size.y = Mathf.Max(0, Size.y);

        Size = new Vector2(size.x, size.y);
        UpdateSize();
        UpdateMovement();
    //    EditorUtility.SetDirty(this);

        return underZero;
    }

    public bool MoveScaleVisualElement(Vector2 moveAmount, Vector2 scaleAmount)
    {
        bool underZero = false;

     //   Undo.RegisterCompleteObjectUndo(this, "Move/Scale Image");

        Position += moveAmount;
        UpdateMovement();

        size += scaleAmount;


        if (size.x <= 0 || size.y <= 0) underZero = true;
        size.x = Mathf.Max(0, size.x);
        size.y = Mathf.Max(0, size.y);

        Size = new Vector2(size.x, size.y);
        UpdateSize();
      //  EditorUtility.SetDirty(this);

        return underZero;
    }



    protected void UpdateMovement()
    {
        Vector2 offSet = new Vector2(50, 50);
        if (visualElement.parent != null) offSet = new Vector2(visualElement.parent.style.width.value.value / 2.0f, visualElement.parent.style.height.value.value / 2.0f);


        visualElement.style.left = Position.x + offSet.x - Size.x / 2.0f;
        visualElement.style.top = Position.y + offSet.y - Size.y / 2.0f;
    }

    private void UpdateSize()
    {
        visualElement.style.width = size.x;
        visualElement.style.height = size.y;
    }



}
