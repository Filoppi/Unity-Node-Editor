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

            public void Render()
            {
                Rect = GUI.Window(Id, Rect, OnRender, Title);
            }

            private void OnRender(int id)
            {
                GUI.DragWindow();
            }
        }

        private class Edge
        {
            public Node A { get; set; }

            public Node B { get; set; }

            public Edge(Node a, Node b)
            {
                A = a;
                B = b;
            }

            public void Render()
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
            GetWindow<NodeEditor>();
        }

        private readonly List<Node> _nodes = new List<Node>();
        private readonly List<Edge> _edges = new List<Edge>(); 

        private NodeEditor()
        {
            Node a = new Node(new Rect(10.0f, 10.0f, 100.0f, 100.0f), "Node A");
            _nodes.Add(a);

            Node b = new Node(new Rect(300.0f, 300.0f, 100.0f, 100.0f), "Node B");
            _nodes.Add(b);

            _edges.Add(new Edge(a, b));
        }

        private void OnGUI()
        {
            foreach(Edge edge in _edges) {
                edge.Render();
            }

            BeginWindows();
                foreach(Node node in _nodes) {
                    node.Render();
                }
            EndWindows();
        }
    }
}
