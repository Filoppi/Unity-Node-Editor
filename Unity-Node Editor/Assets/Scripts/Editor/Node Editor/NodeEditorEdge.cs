using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public abstract class NodeEditorEdge
    {
        private const float ClickEpsilon = 5.0f;

#region Id Generator
        private static int LastId;

        private static int NextId => ++LastId;
#endregion

        public int Id { get; } = NextId;

        public NodeEditorNode A { get; set; }

        public NodeEditorNode B { get; set; }

        private readonly NodeEditor _owner;

        protected NodeEditorEdge(NodeEditorNode a, NodeEditorNode b, NodeEditor owner)
        {
            _owner = owner;

            A = a;
            B = b;
        }

#region TODO: encapsulate this better
        private Vector3 GetStartPosition()
        {
            return new Vector3(A.Rect.x + A.Rect.width, A.Rect.y + A.Rect.height / 2.0f, 0.0f);
        }

        private Vector3 GetEndPosition()
        {
            return new Vector3(B.Rect.x, B.Rect.y + B.Rect.height / 2.0f, 0.0f);
        }

        private Vector3 GetStartTangent(Vector3 startPosition)
        {
            return startPosition + Vector3.right * 50.0f;
        }

        private Vector3 GetEndTangent(Vector3 endPosition)
        {
            return endPosition + Vector3.left * 50.0f;
        }
#endregion

        public bool HandleEvent(Event currentEvent)
        {
            Vector3 startPosition = GetStartPosition();
            Vector3 startTangent = GetStartTangent(startPosition);

            Vector3 endPosition = GetEndPosition();
            Vector3 endTangent = GetEndTangent(endPosition);

            switch(currentEvent.type)
            {
            case EventType.MouseUp:
                if(HandleUtility.DistancePointBezier(currentEvent.mousePosition, startPosition, endPosition, startTangent, endTangent) > ClickEpsilon) {
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
            Color shadowColor = new Color(0.0f, 0.0f, 0.0f, 0.06f);

            Vector3 startPosition = GetStartPosition();
            Vector3 startTangent = GetStartTangent(startPosition);

            Vector3 endPosition = GetEndPosition();
            Vector3 endTangent = GetEndTangent(endPosition);

            // shadow
            for(int i=0; i<3; ++i) {
                Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, shadowColor, null, (i + 1.0f) * 5.0f);
            }
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, Color.black, null, 1.0f);
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
            menu.AddItem(new GUIContent("Delete Edge"), false, OnDelete);
            menu.ShowAsContext();
        }

        private void OnDelete()
        {
            _owner.DeleteEdge(this);
        }
    }
}
