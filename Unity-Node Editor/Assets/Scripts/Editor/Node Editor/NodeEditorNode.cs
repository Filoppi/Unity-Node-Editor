using UnityEngine;

namespace EnergonSoftware.Editor
{
    public abstract class NodeEditorNode
    {
#region Id Generator
        private static int LastId;

        private static int NextId => ++LastId;
#endregion

        public int Id { get; } = NextId;

        public Rect Rect { get; private set; }

        public bool IsResizable { get; set; }

        public string Title { get; set; }

// TODO: resizing

// TODO: add edge connection points (edges should connect to these rather than deriving from the Rect)

        private readonly NodeEditor _owner;

        protected NodeEditorNode(Rect rect, string title, NodeEditor owner)
        {
            _owner = owner;

            Rect = rect;
            Title = title;
        }

        public void AddEdge()
        {
            Debug.Log("TODO: add an edge!");
        }

        public void Delete()
        {
            _owner.DeleteNode(this);
        }

        public bool HandleEvent(Event currentEvent)
        {
            switch(currentEvent.type)
            {
            case EventType.MouseUp:
                if(!Rect.Contains(currentEvent.mousePosition)) {
                    break;
                }

                OnMouseUp(currentEvent.button, currentEvent.mousePosition);
                currentEvent.Use();
                return true;
            }
            return false;
        }

        private void OnMouseUp(int button, Vector2 mousePosition)
        {
            switch(button)
            {
            case 0:
                OnLeftClick(mousePosition);
                break;
            case 1:
                OnRightClick(mousePosition);
                break;
            case 2:
                break;
            }
        }

        protected virtual void OnLeftClick(Vector2 mousePosition)
        {
        }

        protected virtual void OnRightClick(Vector2 mousePosition)
        {
        }

        public void Render(Event currentEvent)
        {
            Rect = GUI.Window(Id, Rect, DoRender, Title);
        }

        private void DoRender(int id)
        {
            OnRender();

            GUI.DragWindow();

// http://forum.unity3d.com/threads/simple-node-editor.189230/#post-1719143 for resize
        }

        protected virtual void OnRender()
        {
        }
    }
}
