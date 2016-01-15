using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class SequenceEditorNode : NodeEditorNode
    {
        public SequenceEditorNode(Rect rect, string title, NodeEditor owner)
            : base(rect, title, owner)
        {
        }
    }

// http://forum.unity3d.com/threads/simple-node-editor.189230/page-3

    [CustomEditor(typeof(SequenceEditorNode))]
    public class TestNodeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
        }
    }
}
