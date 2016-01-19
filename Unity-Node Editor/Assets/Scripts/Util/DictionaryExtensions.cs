using System.Collections.Generic;

namespace EnergonSoftware.Util
{
    public static class DictionaryExtensions
    {
        public static V GetOrDefault<K, V>(this Dictionary<K, V> dict, K key, V defaultValue=default(V))
        {
            V value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
