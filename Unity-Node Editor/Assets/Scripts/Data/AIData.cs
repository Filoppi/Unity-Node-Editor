using System;

using UnityEngine;

namespace EnergonSoftware.Data
{
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
    }
}
