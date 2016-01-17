using UnityEditor;
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
// TODO: add edge connection points

        private readonly NodeEditor _owner;

        protected NodeEditorNode(Rect rect, string title, NodeEditor owner)
        {
            _owner = owner;

            Rect = rect;
            Title = title;
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

        public void Render(Event currentEvent)
        {
            Rect = GUI.Window(Id, Rect, DoRender, Title);
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

        protected void OnLeftClick(Vector2 mousePosition)
        {
        }

        protected void OnRightClick(Vector2 mousePosition)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Edge"), false, OnAddEdge);
            menu.AddItem(new GUIContent("Delete Node"), false, OnDelete);
            menu.ShowAsContext();
        }

        private void OnAddEdge()
        {
            Debug.Log("TODO: add an edge!");
        }

        private void OnDelete()
        {
            _owner.DeleteNode(this);
        }

        private void DoRender(int id)
        {
            GUI.DragWindow();

// http://forum.unity3d.com/threads/simple-node-editor.189230/#post-1719143 for resize
        }
    }
}
