using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public class NodeEditor : EditorWindow
    {
        private class Node
        {
#region Id Generator
            private static int LastId;

            private static int NextId { get { return ++LastId; } }
#endregion

            public int Id { get; private set; }

            public Rect Rect { get; private set; }

            public string Title { get; private set; }

            private readonly NodeEditor _owner;

            public Node(Rect rect, string title, NodeEditor owner)
            {
                Id = NextId;
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
                    OnMouseUp(currentEvent);
                    return true;
                }
                return false;
            }

            public void Render(Event currentEvent)
            {
                Rect = GUI.Window(Id, Rect, DoRender, Title);
            }

            private void OnMouseUp(Event currentEvent)
            {
                switch(currentEvent.button)
                {
                case 0:
                    break;
                case 1:
                    OnContextClick(currentEvent.mousePosition);
                    currentEvent.Use();
                    break;
                case 2:
                    break;
                }
            }

            protected void OnContextClick(Vector2 mousePosition)
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
            }
        }

        private class Edge
        {
            private const float ClickEpsilon = 5.0f;

#region Id Generator
            private static int LastId;

            private static int NextId { get { return ++LastId; } }
#endregion

            public int Id { get; private set; }

            public Node A { get; set; }

            public Node B { get; set; }

            private readonly NodeEditor _owner;

            public Edge(Node a, Node b, NodeEditor owner)
            {
                Id = NextId;
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
                    OnMouseUp(currentEvent);
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

            private void OnMouseUp(Event currentEvent)
            {
                switch(currentEvent.button)
                {
                case 0:
                    break;
                case 1:
                    OnContextClick(currentEvent.mousePosition);
                    currentEvent.Use();
                    break;
                case 2:
                    break;
                }
            }

            protected void OnContextClick(Vector2 mousePosition)
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

        [MenuItem("Energon Software/Node Editor")]
        public static void ShowEditor()
        {
            NodeEditor window = GetWindow<NodeEditor>();
            window.Show();
        }

        private readonly List<Node> _nodes = new List<Node>();
        private readonly List<Edge> _edges = new List<Edge>(); 

        private NodeEditor()
        {
            titleContent = new GUIContent { text = "Node Editor" };

            Node a = new Node(new Rect(10.0f, 10.0f, 100.0f, 100.0f), "Node A", this);
            _nodes.Add(a);

            Node b = new Node(new Rect(300.0f, 300.0f, 100.0f, 100.0f), "Node B", this);
            _nodes.Add(b);

            _edges.Add(new Edge(a, b, this));
        }

        private void DeleteNode(Node node)
        {
            _nodes.Remove(node);
// TODO: update edges
        }

        private void DeleteEdge(Edge edge)
        {
            _edges.Remove(edge);
        }

        private void OnGUI()
        {
            HandleEvent(Event.current);

            //GUI.BeginGroup();

            foreach(Edge edge in _edges) {
                edge.Render(Event.current);
            }

            BeginWindows();
                foreach(Node node in _nodes) {
                    node.Render(Event.current);
                }
            EndWindows();

            //GUI.EndGroup();
        }

        private bool HandleEvent(Event currentEvent)
        {
            foreach(Edge edge in _edges) {
                if(edge.HandleEvent(currentEvent)) {
                    return true;
                }
            }

            foreach(Node node in _nodes) {
                if(node.HandleEvent(currentEvent)) {
                    return true;
                }
            }

            switch(currentEvent.type)
            {
            case EventType.MouseUp:
                OnMouseUp(currentEvent);
                return true;
            }

            return false;
        }

        private void OnMouseUp(Event currentEvent)
        {
            switch(currentEvent.button)
            {
            case 0:
                break;
            case 1:
                OnContextClick(currentEvent.mousePosition);
                currentEvent.Use();
                break;
            case 2:
                break;
            }
        }

        private void OnContextClick(Vector2 mousePosition)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Node"), false, OnAddNode, mousePosition);
            menu.ShowAsContext();
        }

        private void OnAddNode(object obj)
        {
            Vector2 mousePosition = (Vector2)obj;

            Node node = new Node(new Rect(mousePosition.x, mousePosition.y, 100.0f, 100.0f), "Node", this);
            _nodes.Add(node);
        }
    }
}
