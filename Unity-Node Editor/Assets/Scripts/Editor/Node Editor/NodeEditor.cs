using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public abstract class NodeEditor : EditorWindow
    {
        private readonly List<NodeEditorNode> _nodes = new List<NodeEditorNode>();

        private readonly List<NodeEditorEdge> _edges = new List<NodeEditorEdge>();

        private Vector2 _scrollPosition;

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
            foreach(NodeEditorEdge edge in _edges) {
                if(edge.StartNode == node) {
                    edge.DisconnectStart();
                }

                if(edge.EndNode == node) {
                    edge.DisconnectEnd();
                }
            }
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

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            foreach(NodeEditorEdge edge in _edges) {
                edge.Render(Event.current);
            }

            BeginWindows();
                foreach(NodeEditorNode node in _nodes) {
                    node.Render(Event.current);
                }
            EndWindows();

            EditorGUILayout.EndScrollView();
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

        protected virtual void OnLeftClick(Vector2 mousePosition)
        {
        }

        protected virtual void OnRightClick(Vector2 mousePosition)
        {
        }
    }
}
