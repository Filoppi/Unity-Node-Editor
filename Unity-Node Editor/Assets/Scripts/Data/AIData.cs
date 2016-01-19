using System;
using System.Collections.Generic;

using UnityEngine;

namespace EnergonSoftware.Data
{
    [Serializable]
    public class AIState
    {
        public string Id = string.Empty;

        public string Name = string.Empty;

        public string DecisionExpression = string.Empty;

        public float DecisionFrequenceySeconds = 1.0f;

        public string TrueState = string.Empty;

        public string FalseState = string.Empty;

        public Vector2 EditorPosition;
    }

    [Serializable]
    public class AIStateTransition
    {
        public string Id = string.Empty;

        public string Name = string.Empty;

        public string StartState = string.Empty;

        public string EndState = string.Empty;
    }

    [Serializable]
    public class AIData : ScriptableObject
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/EnergonSoftware/AI Data")]
        public static void CreateAsset()
        {
            Editor.ScriptableObjectUtil.CreateAsset<AIData>();
        }
#endif

        public string Id = string.Empty;

        public List<AIState> States = new List<AIState>();

        public List<AIStateTransition> StateTransitions = new List<AIStateTransition>(); 
    }
}
