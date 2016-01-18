using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class AIEditorNode : NodeEditorNode
    {
        public AIEditorNode(Vector2 position, string title, NodeEditor owner)
            : base(new Rect(position.x, position.y, 100.0f, 100.0f), title, owner)
        {
        }

        protected override void OnRightClick(Vector2 mousePosition)
        {
            base.OnRightClick(mousePosition);

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add State Transition"), false, AddEdge);
            menu.AddItem(new GUIContent("Delete State"), false, Delete);
            menu.ShowAsContext();
        }
    }

// http://forum.unity3d.com/threads/simple-node-editor.189230/page-3

    [CustomEditor(typeof(AIEditorNode))]
    public class AINodeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
        }
    }
}
