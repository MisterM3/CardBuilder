using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;

/// <summary>
/// Gizmo where you can move two arrows into two different directions parralel to eachother
/// </summary>
public class XYGizmo : BaseGizmos
{
    private ArrowDown arrowDown = ArrowDown.None;

    private enum ArrowDown
    {
        Vertical,
        Horizontal,
        Both,
        None
    }


    new public void Init(VisualElement rootWindow)
    {
        base.Init(rootWindow);

        SetupVerticalArrow();
        SetupHorizontalArrow();
        SetupBothDirectionsArrow();

        SetupRootWindow();
    }

    #region Activate/Deactivate

    public override void OnActivate()
    {
        base.OnActivate();

        SetupVerticalArrow();
        SetupHorizontalArrow();
        SetupBothDirectionsArrow();

        SetupRootWindow();
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        rootWindow.UnregisterCallback<PointerUpEvent>(((evt) => arrowDown = ArrowDown.None));
        rootWindow.UnregisterCallback<PointerMoveEvent>(ArrowMove);
        rootWindow.UnregisterCallback<PointerLeaveEvent>(((evt) => arrowDown = ArrowDown.None));

        VisualElement verticalArrow = m_gizmoElement.QLogged<VisualElement>("VerticalArrow");
        verticalArrow.UnregisterCallback<PointerDownEvent>((evt) => arrowDown = ArrowDown.Vertical);

        VisualElement horizontalArrow = m_gizmoElement.QLogged<VisualElement>("HorizontalArrow");
        horizontalArrow.UnregisterCallback<PointerDownEvent>((evt) => arrowDown = ArrowDown.Horizontal);

        VisualElement bothArrow = m_gizmoElement.QLogged<VisualElement>("BothArrow");
        bothArrow.UnregisterCallback<PointerDownEvent>((evt) => arrowDown = ArrowDown.Both);
    }

    #endregion

    //Rootwindow used for canceling events and moving
    private void SetupRootWindow()
    {
        rootWindow.RegisterCallback<PointerUpEvent>(((evt) => arrowDown = ArrowDown.None));
        rootWindow.RegisterCallback<PointerMoveEvent>(ArrowMove);
        rootWindow.RegisterCallback<PointerLeaveEvent>(((evt) => arrowDown = ArrowDown.None));
    }

    private void SetupVerticalArrow()
    {
        VisualElement verticalArrow = m_gizmoElement.QLogged<VisualElement>("VerticalArrow");
        verticalArrow.RegisterCallback<PointerDownEvent>((evt) => arrowDown = ArrowDown.Vertical);
    }

    private void SetupHorizontalArrow()
    {
        VisualElement horizontalArrow = m_gizmoElement.QLogged<VisualElement>("HorizontalArrow");
        horizontalArrow.RegisterCallback<PointerDownEvent>((evt) => arrowDown = ArrowDown.Horizontal);
    }
    private void SetupBothDirectionsArrow()
    {
        VisualElement bothArrow = m_gizmoElement.QLogged<VisualElement>("BothArrow");
        bothArrow.RegisterCallback<PointerDownEvent>((evt) => arrowDown = ArrowDown.Both);
    }

    protected void ArrowMove(PointerMoveEvent evt) 
    {
        switch (arrowDown)
        {
            case ArrowDown.Vertical:
                OnArrowMove(new Vector2(0, evt.deltaPosition.y));
                break;
            case ArrowDown.Horizontal:
                OnArrowMove(new Vector2(evt.deltaPosition.x, 0));
                break;
            case ArrowDown.Both:
                OnArrowMove(evt.deltaPosition);
                break;
            default:
                break;
        }
    }

    //Maybe rename
    protected virtual void OnArrowMove(Vector2 delta) { }

}
