﻿using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class SequenceEditorEdge : NodeEditorEdge
    {
        public SequenceEditorEdge(NodeEditor owner)
            : base(owner)
        {
        }

        protected override void OnRightClick(Vector2 mousePosition)
        {
            base.OnRightClick(mousePosition);

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete Sequence Connection"), false, Delete);
            menu.ShowAsContext();
        }
    }
}
