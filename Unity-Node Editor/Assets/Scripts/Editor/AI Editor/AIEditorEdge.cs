using UnityEditor;
using UnityEngine;

namespace EnergonSoftware.Editor
{
    public sealed class AIEditorEdge : NodeEditorEdge
    {
        public AIEditorEdge(NodeEditor owner)
            : base(owner)
        {
        }

        protected override void OnRightClick(Vector2 mousePosition)
        {
            base.OnRightClick(mousePosition);

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete State Transition"), false, Delete);
            menu.ShowAsContext();
        }
    }
}
