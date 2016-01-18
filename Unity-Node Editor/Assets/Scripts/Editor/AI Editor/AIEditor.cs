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
            AIEditorNode a = new AIEditorNode(new Vector2(10.0f, 10.0f), "Action A", this);
            AddNode(a);

            AIEditorNode b = new AIEditorNode(new Vector2(300.0f, 300.0f), "Action B", this);
            AddNode(b);

            AIEditorEdge edge = new AIEditorEdge(this)
            {
                StartNode = a,
                EndNode = b
            };
            AddEdge(edge);
#endregion
        }
    }
}
