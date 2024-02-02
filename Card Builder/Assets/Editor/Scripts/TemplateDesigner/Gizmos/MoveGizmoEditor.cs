using CardBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Gizmo to move visualelement
/// </summary>
public class MoveGizmoEditor : XYGizmo
{
    protected override void OnArrowMove(Vector2 delta)
    {
        if (TargetElement == null)
        {
            Logs.Error("MoveGizmo has no targetElement");
            return;
        }

        TargetElement.MoveVisualElement(delta);
    }
}
