using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class SequenceEditorNode : NodeEditorNode
    {
        public SequenceEditorNode(Vector2 position, string title, NodeEditor owner)
            : base(new Rect(position.x, position.y, 300.0f, 200.0f), title, owner)
        {
        }

        protected override void OnRender()
        {
            GUILayout.Label("Test Content");
        }

        protected override void OnRightClick(Vector2 mousePosition)
        {
            base.OnRightClick(mousePosition);

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Sequence Connection"), false, AddEdge);
            menu.AddItem(new GUIContent("Delete Sequence Action"), false, Delete);
            menu.ShowAsContext();
        }
    }

// http://forum.unity3d.com/threads/simple-node-editor.189230/page-3

    [CustomEditor(typeof(SequenceEditorNode))]
    public class SequenceNodeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
        }
    }
}
