using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public class NodeEditor : EditorWindow
    {
        private class Node
        {
            public Rect Rect { get; private set; }
            
            public Node(Rect rect)
            {
                Rect = rect;
            }
            
            public void Render()
            {
                Rect = GUI.Window(1, Rect, OnRender, "Test Node");
            }
            
            private void OnRender(int id)
            {
                GUI.DragWindow();
            }
        }
        
        [MenuItem("Energon Software/Node Editor")]
        public static void ShowEditor()
        {
            NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
        }
        
        private readonly List<Node> _nodes = new List<Node>();
        
        private NodeEditor()
        {
            _nodes.Add(new Node(new Rect(10, 10, 100, 100)));
        }
        
        private void OnGUI()
        {
            BeginWindows();
                foreach(Node node in _nodes) {
                    node.Render();
                }
            EndWindows();
        }
    }
}
