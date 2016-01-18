using EnergonSoftware.Data;

using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class AIEditor : NodeEditor
    {
        private const string TestAssetPath = "Assets/Data/test_ai.asset";

        [MenuItem("Energon Software/AI Editor")]
        public static void ShowEditor()
        {
            AIEditor window = GetWindow<AIEditor>();
            window.Show();
        }

        public AIEditor()
            : base("AI Editor")
        {
        }

        private void OnEnable()
        {
            LoadData();
        }

        private void LoadData()
        {
            Debug.Log($"Loading AI data from '{TestAssetPath}'...");

            AIData data = AssetDatabase.LoadAssetAtPath<AIData>(TestAssetPath);
            if(null == data) {
                Debug.LogError("Could not load AI data!");
                return;
            }

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

        protected override void OnRightClick(Vector2 mousePosition)
        {
            base.OnRightClick(mousePosition);

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add State"), false, OnAddNode, mousePosition);
            menu.ShowAsContext();
        }

        private void OnAddNode(object obj)
        {
            Vector2 mousePosition = (Vector2)obj;

            NodeEditorNode node = new SequenceEditorNode(mousePosition, "State", this);
            AddNode(node);
        }
    }
}
