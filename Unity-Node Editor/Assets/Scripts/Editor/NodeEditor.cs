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

            public void Render(Event currentEvent)
            {
                HandleEvent(currentEvent);

                Rect = GUI.Window(Id, Rect, DoRender, Title);
            }

            private void HandleEvent(Event currentEvent)
            {
                switch(currentEvent.type)
                {
                case EventType.MouseUp:
                    if(!Rect.Contains(currentEvent.mousePosition)) {
                        break;
                    }
                    OnMouseUp(currentEvent);
                    break;
                }
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
                menu.AddItem(new GUIContent("Delete"), false, OnDelete);
                menu.ShowAsContext();
            }

            private void DoRender(int id)
            {
                GUI.DragWindow();
            }

            private void OnDelete()
            {
                _owner.DeleteNode(this);
            }
        }

        private class Edge
        {
#region Id Generator
            private static int LastId;

            private static int NextId { get { return ++LastId; } }
#endregion

            public int Id { get; private set; }

            public Node A { get; set; }

            public Node B { get; set; }

            private readonly NodeEditor _owner;

            private int _controlId;

            public Edge(Node a, Node b, NodeEditor owner)
            {
                Id = NextId;
                _owner = owner;

                A = a;
                B = b;
            }

            public void Render(Event currentEvent)
            {
                _controlId = GUIUtility.GetControlID(Id, FocusType.Passive);

                HandleEvent(currentEvent);

                Color shadowCol = new Color(0.0f, 0.0f, 0.0f, 0.06f);

                Vector3 start = new Vector3(A.Rect.x + A.Rect.width, A.Rect.y + A.Rect.height / 2.0f, 0.0f);
                Vector3 startTan = start + Vector3.right * 50.0f;

                Vector3 end = new Vector3(B.Rect.x, B.Rect.y + B.Rect.height / 2.0f, 0.0f);
                Vector3 endTan = end + Vector3.left * 50.0f;

                // shadow
                for(int i=0; i<3; ++i) {
                    Handles.DrawBezier(start, end, startTan, endTan, shadowCol, null, (i + 1.0f) * 5.0f);
                }
                Handles.DrawBezier(start, end, startTan, endTan, Color.black, null, 1.0f);
            }

            private void HandleEvent(Event currentEvent)
            {
                if(GUIUtility.hotControl != _controlId) {
                    return;
                }

                EventType type = currentEvent.GetTypeForControl(_controlId);
                switch(type)
                {
                case EventType.MouseUp:
                    OnMouseUp(currentEvent);
                    break;
                }
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
                menu.AddItem(new GUIContent("Delete"), false, OnDelete);
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

            if(EventType.MouseUp == Event.current.type && 1 == Event.current.button) {
                OnContextClick(Event.current.mousePosition);
                Event.current.Use();
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
