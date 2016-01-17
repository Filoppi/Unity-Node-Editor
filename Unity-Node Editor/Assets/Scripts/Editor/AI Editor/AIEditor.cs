using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class AIEditor : NodeEditor
    {
        [MenuItem("Energon Software/AI Editor")]
        public static void ShowEditor()
        {
            AIEditor window = GetWindow<AIEditor>();
            window.Show();
        }

        public AIEditor()
            : base("AI Editor")
        {
#region TEST JUNK PLEASE REMOVE
            SequenceEditorNode a = new SequenceEditorNode(new Rect(10.0f, 10.0f, 100.0f, 100.0f), "Node A", this);
            AddNode(a);

            SequenceEditorNode b = new SequenceEditorNode(new Rect(300.0f, 300.0f, 100.0f, 100.0f), "Node B", this);
            AddNode(b);

            SequenceEditorEdge edge = new SequenceEditorEdge(a, b, this);
            AddEdge(edge);
#endregion
        }
    }
}
