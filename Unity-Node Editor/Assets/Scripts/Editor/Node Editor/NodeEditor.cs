using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public abstract class NodeEditor : EditorWindow
    {
        private readonly List<NodeEditorNode> _nodes = new List<NodeEditorNode>();

        private readonly List<NodeEditorEdge> _edges = new List<NodeEditorEdge>(); 

        protected NodeEditor(string title)
        {
            titleContent = new GUIContent { text = title };
        }

        protected void AddNode(NodeEditorNode node)
        {
            _nodes.Add(node);
        }

        public void DeleteNode(NodeEditorNode node)
        {
            _nodes.Remove(node);
// TODO: update edges
        }

        protected void AddEdge(NodeEditorEdge edge)
        {
            _edges.Add(edge);
        }

        public void DeleteEdge(NodeEditorEdge edge)
        {
            _edges.Remove(edge);
        }

        private void OnGUI()
        {
            HandleEvent(Event.current);

            //GUI.BeginGroup();

            foreach(NodeEditorEdge edge in _edges) {
                edge.Render(Event.current);
            }

            BeginWindows();
                foreach(NodeEditorNode node in _nodes) {
                    node.Render(Event.current);
                }
            EndWindows();

            //GUI.EndGroup();
        }

        private bool HandleEvent(Event currentEvent)
        {
            foreach(NodeEditorEdge edge in _edges) {
                if(edge.HandleEvent(currentEvent)) {
                    return true;
                }
            }

            foreach(NodeEditorNode node in _nodes) {
                if(node.HandleEvent(currentEvent)) {
                    return true;
                }
            }

            switch(currentEvent.type)
            {
            case EventType.MouseUp:
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

        private void OnLeftClick(Vector2 mousePosition)
        {
        }

        private void OnRightClick(Vector2 mousePosition)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Node"), false, OnAddNode, mousePosition);
            menu.ShowAsContext();
        }

        private void OnAddNode(object obj)
        {
            Vector2 mousePosition = (Vector2)obj;

            /*NodeEditorNode node = new NodeEditorNode(new Rect(mousePosition.x, mousePosition.y, 100.0f, 100.0f), "Node", this);
            _nodes.Add(node);*/
        }
    }
}
