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

            public Node(Rect rect, string title)
            {
                Id = NextId;

                Rect = rect;
                Title = title;
            }

            public bool HandlEvent(Event currentEvent)
            {
                if(EventType.MouseUp == currentEvent.type && 1 == currentEvent.button && Rect.Contains(currentEvent.mousePosition)) {
                    OnContextClick(currentEvent.mousePosition);
                    currentEvent.Use();
                    return true;
                }

                return false;
            }

            public void Render()
            {
                Rect = GUI.Window(Id, Rect, OnRender, Title);
            }

            protected void OnContextClick(Vector2 mousePosition)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Delete"), false, OnDelete);
                menu.ShowAsContext();
            }

            private void OnRender(int id)
            {
                GUI.DragWindow();
            }

            private void OnDelete()
            {
Debug.Log("Delete node " + Id);
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

            public Edge(Node a, Node b)
            {
                Id = NextId;

                A = a;
                B = b;
            }

            public bool HandlEvent(Event currentEvent)
            {
                return false;
            }

            public void Render(Event currentEvent)
            {
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

            Node a = new Node(new Rect(10.0f, 10.0f, 100.0f, 100.0f), "Node A");
            _nodes.Add(a);

            Node b = new Node(new Rect(300.0f, 300.0f, 100.0f, 100.0f), "Node B");
            _nodes.Add(b);

            _edges.Add(new Edge(a, b));
        }

        private void OnGUI()
        {
#region Handle events
            bool eventHandled = false;
            foreach(Edge edge in _edges) {
                if(edge.HandlEvent(Event.current)) {
                    eventHandled = true;
                    break;
                }
            }

            foreach(Node node in _nodes) {
                if(!eventHandled && node.HandlEvent(Event.current)) {
                    eventHandled = true;
                    break;
                }
            }

            if(!eventHandled && EventType.MouseUp == Event.current.type && 1 == Event.current.button) {
                OnContextClick(Event.current.mousePosition);
                Event.current.Use();
                eventHandled = true;
            }
#endregion

#region Render
            foreach(Edge edge in _edges) {
                edge.Render(Event.current);
            }

            BeginWindows();
                foreach(Node node in _nodes) {
                    node.Render();
                }
            EndWindows();
#endregion
        }

        private void OnContextClick(Vector2 mousePosition)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Node"), false, OnAddNode);
            menu.ShowAsContext();
        }

        private void OnAddNode()
        {
Debug.Log("Add a new node!");
        }
    }
}
