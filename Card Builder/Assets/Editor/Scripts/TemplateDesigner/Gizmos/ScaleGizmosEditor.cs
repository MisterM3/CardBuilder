using CardBuilder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScaleGizmosEditor : XYGizmo
{
    protected override void OnArrowMove(Vector2 delta)
    {
        if (TargetElement == null)
        {
            Logs.Error("MoveGizmo has no targetElement");
            return;
        }

        delta.y *= -1;

        if (!TargetElement.ScaleVisualElement(delta))
        {
            TargetElement.MoveVisualElement(-(delta / 4.0f));
        }
        SetGizmoToTargetPosition();
    }
}
