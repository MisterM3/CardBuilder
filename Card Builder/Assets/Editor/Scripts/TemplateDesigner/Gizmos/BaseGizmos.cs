using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Base class for gizmos used to move and scale visualelements in the editorwindow
/// </summary>
public class BaseGizmos : ScriptableObject
{
    [SerializeField]
    protected VisualTreeAsset m_Gizmos = default;

    protected VisualElement m_gizmoElement;

    //Element to move
    private UndoRedoVisualElement targetElement = null;

    protected VisualElement rootWindow = null;



    public UndoRedoVisualElement TargetElement
    {
        protected get
        {
            return targetElement;
        }
        set
        {
            targetElement = value;
            targetElement.VisualElement.Add(m_gizmoElement);
            SetGizmoToTargetPosition();
        }
    }

    public virtual void Init(VisualElement rootWindow)
    {
        this.rootWindow = rootWindow;
        m_gizmoElement = m_Gizmos.CloneTree();

    }

    #region Activate/Deactivate

    public virtual void OnActivate()
    {
        if (TargetElement != null)
            TargetElement.VisualElement.Add(m_gizmoElement);
    }

    public virtual void OnDeactivate()
    {
        if (TargetElement != null)
            TargetElement.VisualElement.Remove(m_gizmoElement);
    }

    #endregion

    protected virtual void SetGizmoToTargetPosition()
    {
        float middleTargetElementX = TargetElement.VisualElement.style.width.value.value / 2 - 5;
        float middleTargetElementY = -TargetElement.VisualElement.style.height.value.value / 2 + 50;

        m_gizmoElement.style.left = middleTargetElementX;
        m_gizmoElement.style.top = -middleTargetElementY;
    }

    public void UndoReset()
    {
        SetGizmoToTargetPosition();
    }
}
