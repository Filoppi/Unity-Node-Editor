﻿using UnityEditor;
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
            SequenceEditorNode a = new SequenceEditorNode(new Rect(10.0f, 10.0f, 100.0f, 100.0f), "Node A", this);
            AddNode(a);

            SequenceEditorNode b = new SequenceEditorNode(new Rect(300.0f, 300.0f, 100.0f, 100.0f), "Node B", this);
            AddNode(b);

            SequenceEditorEdge edge = new SequenceEditorEdge(a, b, this);
            AddEdge(edge);
        }
    }
}