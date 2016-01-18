using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public enum NodeEditorEdgeType
    {
        Line,
        Bezier
    }

    public abstract class NodeEditorEdge
    {
        private const float ClickEpsilon = 5.0f;

#region Id Generator
        private static int LastId;

        private static int NextId => ++LastId;
#endregion

        public int Id { get; } = NextId;

        public NodeEditorEdgeType Type { get; set; } = NodeEditorEdgeType.Line;

// TODO: add arrow direction indicators

#region Start
        public NodeEditorNode StartNode { get; set; }

        public Vector3 StartPosition { get; set; }

// TODO: this should use the node connection points
        private Vector3 RenderStart => null == StartNode ? StartPosition : new Vector3(StartNode.Rect.x + StartNode.Rect.width, StartNode.Rect.y + StartNode.Rect.height / 2.0f);

        private Vector3 StartBezierTangent => RenderStart + Vector3.right * 50.0f;
#endregion

#region End
        public NodeEditorNode EndNode { get; set; }

        public Vector3 EndPosition { get; set; }

// TODO: this should use the node connection points
        private Vector3 RenderEnd => null == EndNode ? EndPosition : new Vector3(EndNode.Rect.x, EndNode.Rect.y + EndNode.Rect.height / 2.0f);

        private Vector3 EndBezierTangent => RenderEnd + Vector3.left * 50.0f;
#endregion

#region Shadow
        public bool HasShadow { get; set; } = true;

        public Color ShadowColor { get; set; } = Color.gray;
#endregion

        public Color Color { get; } = Color.black;

        private readonly NodeEditor _owner;

        protected NodeEditorEdge(NodeEditor owner)
        {
            _owner = owner;
        }

#region Disconnect
        public void DisconnectStart()
        {
            if(null == StartNode) {
                return;
            }

// TODO: this should use the node connection points
            StartPosition = new Vector3(StartNode.Rect.x + StartNode.Rect.width, StartNode.Rect.y + StartNode.Rect.height / 2.0f);
            StartNode = null;
        }

        public void DisconnectEnd()
        {
            if(null == EndNode) {
                return;
            }

// TODO: this should use the node connection points
            EndPosition = new Vector3(EndNode.Rect.x, EndNode.Rect.y + EndNode.Rect.height / 2.0f);
            EndNode = null;
        }
#endregion

        public void Delete()
        {
            _owner.DeleteEdge(this);
        }

        public bool IsPointCloseTo(Vector2 point)
        {
            switch(Type)
            {
            case NodeEditorEdgeType.Line:
                return HandleUtility.DistancePointLine(point, RenderStart, RenderEnd) < ClickEpsilon;
            case NodeEditorEdgeType.Bezier:
                return HandleUtility.DistancePointBezier(point, RenderStart, RenderEnd, StartBezierTangent, EndBezierTangent) < ClickEpsilon;
            }
            return false;
        }

        public bool HandleEvent(Event currentEvent)
        {
            switch(currentEvent.type)
            {
            case EventType.MouseUp:
                if(!IsPointCloseTo(currentEvent.mousePosition)) {
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
            switch(Type)
            {
            case NodeEditorEdgeType.Line:
                RenderLine();
                break;
            case NodeEditorEdgeType.Bezier:
                RenderBezier();
                break;
            }
        }

        private void RenderLine()
        {
            Vector3 startPosition = RenderStart;
            Vector3 endPosition = RenderEnd;

            // TODO: shadow!

            Color oldColor = Handles.color;
            Handles.color = Color;
            Handles.DrawLine(startPosition, endPosition);
            Handles.color = oldColor;
        }

        private void RenderBezier()
        {
            Vector3 startPosition = RenderStart;
            Vector3 startTangent = StartBezierTangent;

            Vector3 endPosition = RenderEnd;
            Vector3 endTangent = EndBezierTangent;

            if(HasShadow) {
                for(int i=0; i<3; ++i) {
                    Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, ShadowColor, null, (i + 1.0f) * 5.0f);
                }
            }
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, Color, null, 1.0f);
        }
    }
}
