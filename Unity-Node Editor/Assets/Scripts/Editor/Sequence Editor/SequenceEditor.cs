using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class SequenceEditor : NodeEditor
    {
        [MenuItem("Energon Software/Sequence Editor")]
        public static void ShowEditor()
        {
            SequenceEditor window = GetWindow<SequenceEditor>();
            window.Show();
        }

        public SequenceEditor()
            : base("Sequence Editor")
        {
#region TEST JUNK PLEASE REMOVE
            SequenceEditorNode a = new SequenceEditorNode(new Vector2(10.0f, 10.0f), "Action A", this);
            AddNode(a);

            SequenceEditorNode b = new SequenceEditorNode(new Vector2(300.0f, 300.0f), "Action B", this);
            AddNode(b);

            SequenceEditorEdge edge = new SequenceEditorEdge(this)
            {
                StartNode = a,
                EndNode = b
            };
            AddEdge(edge);
#endregion
        }

        protected override void OnRightClick(Vector2 mousePosition)
        {
            base.OnRightClick(mousePosition);

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Sequence Action"), false, OnAddNode, mousePosition);
            menu.ShowAsContext();
        }

        private void OnAddNode(object obj)
        {
            Vector2 mousePosition = (Vector2)obj;

            NodeEditorNode node = new SequenceEditorNode(mousePosition, "Action", this);
            AddNode(node);
        }
    }
}
