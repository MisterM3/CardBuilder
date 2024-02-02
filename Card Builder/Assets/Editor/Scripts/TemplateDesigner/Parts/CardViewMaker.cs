using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Linq;

namespace CardBuilder
{
    public class CardViewMaker : ScriptableObject
    {
        private Dictionary<GizmoEnums, BaseGizmos> gizmoDictionary = new();

        private BaseGizmos selectedGizmo = null;

        private UndoRedoVisualElement selectedImage;

        public UndoRedoVisualElement SelectedImage
        {
            get => selectedImage;
            set
            {
                if (selectedGizmo != null)
                {
                    Undo.RecordObjects(new Object[2] { this, selectedGizmo }, "New SelectedImage: " + value.ToString());
                    selectedGizmo.TargetElement = value;
                    selectedImage = value;
                }
                else
                {
                    Undo.RecordObject(this, "New SelectedImage: " + value.ToString());
                    selectedImage = value;
                }
            }
        }



        public void OnGUI()
        {
            if (Event.current.type == EventType.ValidateCommand)
            {
                switch (Event.current.commandName)
                {
                    case "UndoRedoPerformed":
                        if (selectedImage == null) return;
                        selectedImage.MoveVisualElement(Vector2.zero);
                        selectedImage.ScaleVisualElement(Vector2.zero);
                        if (selectedGizmo != null) selectedGizmo.UndoReset();
                        break;
                }
            }

            if (Event.current.type == EventType.KeyDown)
            {
                if (selectedImage == null) return;

                switch (Event.current.character)
                {
                    case '1':
                        RemoveGizmos();
                        break;
                    case '2':
                        SetActiveGizmo(GizmoEnums.MoveTool);
                        break;
                    case '3':
                        SetActiveGizmo(GizmoEnums.XYScale);
                        break;
                    case '4':
                        SetActiveGizmo(GizmoEnums.FourBoxScale);
                        break;
                }
            }
        }

        public void Remove()
        {
            selectedImage = null;
        }

        public void SetActiveGizmo(GizmoEnums newgizmo)
        {

            if (selectedGizmo != null) selectedGizmo.OnDeactivate();
            selectedGizmo = gizmoDictionary[newgizmo];
            selectedGizmo.TargetElement = selectedImage;
            if (selectedGizmo != null) selectedGizmo.OnActivate();
        }

        public void RemoveGizmos()
        {
            if (selectedGizmo != null) selectedGizmo.OnDeactivate();
            selectedGizmo = null;
           // this.SelectedImage = null;
            //Info on removing gizmo;
        }

        public void Initialize(VisualElement cardWindow)
        {

            // Each editor window contains a root VisualElement object
            VisualElement root = cardWindow;

            MoveGizmoEditor moveGizmo = CreateInstance<MoveGizmoEditor>();
            ScaleGizmosEditor scaleGizmo = CreateInstance<ScaleGizmosEditor>();
            RectToolGizmo rectToolGizmo = CreateInstance<RectToolGizmo>();

            gizmoDictionary.Add(GizmoEnums.MoveTool, moveGizmo);
            gizmoDictionary.Add(GizmoEnums.XYScale, scaleGizmo);
            gizmoDictionary.Add(GizmoEnums.FourBoxScale, rectToolGizmo);


            foreach (BaseGizmos gizmo in gizmoDictionary.Values)
            {
                gizmo.Init(root);
            }

            root.style.flexShrink = 0;
            root.style.flexGrow = 1;
        }

        public void ChangeElement(IEnumerable<object> obj)
        {

            if (obj == null) return;
            if (obj.Count() == 0) return;
            var item = obj.First();

            if (item is not HierarchyData data) return;
            
            if (data.VisualElement == null) return;

            if (data.Name == "ROOT")
            {
                //  this.SelectedImage = null;
                selectedImage = null;
                RemoveGizmos();
                return;
            }

            this.SelectedImage = data.VisualElement;
        }
    }

}
