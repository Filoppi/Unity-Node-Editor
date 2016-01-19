using System.Collections.Generic;

using EnergonSoftware.Data;
using EnergonSoftware.Util;

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

        private readonly Dictionary<string, AIEditorNode> _states = new Dictionary<string, AIEditorNode>(); 

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

            foreach(AIState state in data.States) {
                AIEditorNode stateNode = new AIEditorNode(state.EditorPosition, state.Name, this);
                AddNode(stateNode);

                // TODO: check for overwrites
                _states[state.Id] = stateNode;
            }

            foreach(AIStateTransition stateTransition in data.StateTransitions) {
                AIEditorNode startNode = _states.GetOrDefault(stateTransition.StartState);
                if(null == startNode) {
                    Debug.Log($"No such start state {stateTransition.StartState}!");
                    continue;
                }

                AIEditorNode endNode = _states.GetOrDefault(stateTransition.EndState);
                if(null == endNode) {
                    Debug.Log($"No such end state {stateTransition.EndState}!");
                    continue;
                }

                AIEditorEdge stateEdge = new AIEditorEdge(this)
                {
                    StartNode = startNode,
                    EndNode = endNode
                };
                AddEdge(stateEdge);
            }
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
