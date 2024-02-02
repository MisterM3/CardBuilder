using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;
/// <summary>
/// Gizmo that works like the rectTool but now for the editorwindow
/// </summary>
public class RectToolGizmo : BaseGizmos
{

    private enum RectToolMode
    {
        LeftScale,
        RightScale,
        TopScale,
        BottomScale,
        LeftTopScale,
        LeftBottomScale,
        RightTopScale,
        RightBottomScale,
        PositionMove,
        None
    }

    private RectToolMode currentToolMode = RectToolMode.None;



    public override void Init(VisualElement rootWindow)
    {
        base.Init(rootWindow);

        SetupBars();
        SetupCorners();
        SetupRootWindow();
    }

    #region Activate/Deactivate

    public override void OnActivate()
    {
        base.OnActivate();

        if (TargetElement != null)

        SetupBars();
        SetupCorners();
        SetupRootWindow();
    }
    public override void OnDeactivate()
    {
        base.OnDeactivate();

        TargetElement.VisualElement.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.PositionMove);

        rootWindow.UnregisterCallback<PointerUpEvent>(((evt) => currentToolMode = RectToolMode.None));
        rootWindow.UnregisterCallback<PointerMoveEvent>(RectMove);
        rootWindow.UnregisterCallback<PointerLeaveEvent>(((evt) => currentToolMode = RectToolMode.None));


        VisualElement topleftPull = m_gizmoElement.QLogged<VisualElement>("TopLeftPull");
        topleftPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.LeftTopScale);

        VisualElement toprightPull = m_gizmoElement.QLogged<VisualElement>("TopRightPull");
        toprightPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.RightTopScale);

        VisualElement bottomleftPull = m_gizmoElement.QLogged<VisualElement>("BottomLeftPull");
        bottomleftPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.LeftBottomScale);

        VisualElement bottomrightPull = m_gizmoElement.QLogged<VisualElement>("BottomRightPull");
        bottomrightPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.RightBottomScale);


        VisualElement leftPull = m_gizmoElement.QLogged<VisualElement>("LeftPull");
        leftPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.LeftScale);

        VisualElement rightPull = m_gizmoElement.QLogged<VisualElement>("RightPull");
        rightPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.RightScale);

        VisualElement bottomPull = m_gizmoElement.QLogged<VisualElement>("BottomPull");
        bottomPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.BottomScale);

        VisualElement topPull = m_gizmoElement.QLogged<VisualElement>("TopPull");
        topPull.UnregisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.TopScale);

    }

    #endregion

    protected override void SetGizmoToTargetPosition()
    {
        VisualElement rootOfTool = m_gizmoElement.QLogged<VisualElement>("RootOfTool");

        rootOfTool.style.width = TargetElement.Size.x;
        rootOfTool.style.height = TargetElement.Size.y;


        VisualElement rightPull = m_gizmoElement.QLogged<VisualElement>("RightPull");
        rightPull.style.left = ((TargetElement.Size.x) - ((rightPull.style.width.value.value / 2)));
        rightPull.style.top = -((TargetElement.Size.y));

        VisualElement bottomPull = m_gizmoElement.QLogged<VisualElement>("BottomPull");
        bottomPull.style.top = -((TargetElement.Size.y) + ((bottomPull.style.height.value.value / 2)));

        VisualElement topPull = m_gizmoElement.QLogged<VisualElement>("TopPull");
        topPull.style.top = -((TargetElement.Size.y * 2) + 9);


        VisualElement topleftPull = m_gizmoElement.QLogged<VisualElement>("TopLeftPull");
        topleftPull.style.left = -5;
        topleftPull.style.top = -(TargetElement.Size.y * 2) - 17;

        VisualElement toprightPull = m_gizmoElement.QLogged<VisualElement>("TopRightPull");
        toprightPull.style.left = TargetElement.Size.x - 2;
        toprightPull.style.top = -(TargetElement.Size.y * 2) - 27;

        VisualElement bottomleftPull = m_gizmoElement.QLogged<VisualElement>("BottomLeftPull");
        bottomleftPull.style.left =  -5;
        bottomleftPull.style.top = -(TargetElement.Size.y) - 35;

        VisualElement bottomrightPull = m_gizmoElement.QLogged<VisualElement>("BottomRightPull");
        bottomrightPull.style.left = TargetElement.Size.x - 2;
        bottomrightPull.style.top = -(TargetElement.Size.y) - 45;

    }
        


    private void SetupRootWindow()
    {
        rootWindow.RegisterCallback<PointerUpEvent>(((evt) => currentToolMode = RectToolMode.None));
        rootWindow.RegisterCallback<PointerMoveEvent>(RectMove);
        rootWindow.RegisterCallback<PointerLeaveEvent>(((evt) => currentToolMode = RectToolMode.None));
    }

    private void RectMove(PointerMoveEvent evt)
    {

        switch (currentToolMode)
        {
            case RectToolMode.LeftScale:
                ScaleLeft(evt.deltaPosition.x);
                break;
            case RectToolMode.RightScale:
                ScaleRight(evt.deltaPosition.x);
                break;
            case RectToolMode.TopScale:
                ScaleTop(evt.deltaPosition.y);
                break;
            case RectToolMode.BottomScale:
                ScaleBottom(evt.deltaPosition.y);
                break;
            case RectToolMode.LeftTopScale:
                ScaleLeft(evt.deltaPosition.x);
                ScaleTop(evt.deltaPosition.y);
                break;
            case RectToolMode.LeftBottomScale:
                ScaleLeft(evt.deltaPosition.x);
                ScaleBottom(evt.deltaPosition.y);
                break;
            case RectToolMode.RightTopScale:
                ScaleRight(evt.deltaPosition.x);
                ScaleTop(evt.deltaPosition.y);
                break;
            case RectToolMode.RightBottomScale:
                ScaleRight(evt.deltaPosition.x);
                ScaleBottom(evt.deltaPosition.y);
                break;
            case RectToolMode.PositionMove:
                TargetElement.MoveVisualElement(evt.deltaPosition);
                SetGizmoToTargetPosition();
                break;
            case RectToolMode.None:
                break;
        }
    }


    private void ScaleLeft(float delta)
    {
        TargetElement.MoveScaleVisualElement(new Vector2(delta * .5f, 0), new Vector2(-delta, 0));
        SetGizmoToTargetPosition();
    }

    private void ScaleRight(float delta)
    {
        TargetElement.MoveScaleVisualElement(new Vector2(delta * .5f, 0), new Vector2(delta, 0));
        SetGizmoToTargetPosition();
    }

    private void ScaleTop(float delta)
    {
        TargetElement.MoveScaleVisualElement(new Vector2(0, delta * .5f), new Vector2(0, -delta));
        SetGizmoToTargetPosition();
    }

    private void ScaleBottom(float delta)
    {
        TargetElement.MoveScaleVisualElement(new Vector2(0, delta * .5f), new Vector2(0, delta));
        SetGizmoToTargetPosition();
    }


    private void SetupCorners()
    {
        VisualElement topleftPull = m_gizmoElement.QLogged<VisualElement>("TopLeftPull");
        topleftPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.LeftTopScale);

        VisualElement toprightPull = m_gizmoElement.QLogged<VisualElement>("TopRightPull");
        toprightPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.RightTopScale);

        VisualElement bottomleftPull = m_gizmoElement.QLogged<VisualElement>("BottomLeftPull");
        bottomleftPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.LeftBottomScale);

        VisualElement bottomrightPull = m_gizmoElement.QLogged<VisualElement>("BottomRightPull");
        bottomrightPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.RightBottomScale);

    }

    private void SetupBars()
    {
        VisualElement leftPull = m_gizmoElement.QLogged<VisualElement>("LeftPull");
        leftPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.LeftScale);

        VisualElement rightPull = m_gizmoElement.QLogged<VisualElement>("RightPull");
        rightPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.RightScale);

        VisualElement bottomPull = m_gizmoElement.QLogged<VisualElement>("BottomPull");
        bottomPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.BottomScale);

        VisualElement topPull = m_gizmoElement.QLogged<VisualElement>("TopPull");
        topPull.RegisterCallback<PointerDownEvent>((evt) => currentToolMode = RectToolMode.TopScale);
    }
}
