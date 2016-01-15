using System.IO;

using UnityEngine;
 
namespace EnergonSoftware.Editor
{
#if UNITY_EDITOR
    // http://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
    public static class ScriptableObjectUtil
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static void CreateAsset<T> () where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
 
            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            if(string.IsNullOrEmpty(path)) {
                path = "Assets";
            } else if(!string.IsNullOrEmpty(Path.GetExtension(path))) {
                string filename = Path.GetFileName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject));
                path = path.Replace(filename ?? string.Empty, string.Empty);
            }
 
            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, $"New {typeof(T).Name}.asset"));
            UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);
 
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = asset;
        }
    }
#endif
}
