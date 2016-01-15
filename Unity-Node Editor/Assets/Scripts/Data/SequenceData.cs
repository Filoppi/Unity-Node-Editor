using System;

using UnityEngine;

namespace EnergonSoftware.Data
{
    [Serializable]
    public class SequenceData : ScriptableObject
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/EnergonSoftware/Sequence Data")]
        public static void CreateAsset()
        {
            Editor.ScriptableObjectUtil.CreateAsset<SequenceData>();
        }
#endif

        public string sequenceName = string.Empty;

        [Range(0.0f, 100.0f)]
        public int priority = 0;
    }
}
