using UnityEngine;
using UnityEditor;
using System.IO;
using MOT.Common;

namespace MOT.Editor
{
    /// <summary>
    /// Contains the asset creation menus
    /// </summary>
    public static class CreateMenu
    {
        /// <summary>
        /// Creates a version info asset
        /// </summary>
        [MenuItem("Assets/Create/Mist of Time/Version Info")]
        public static void CreateVersionInfoAsset()
        {
            CreateScriptableObjectAsset<VersionInfo>();
        }

        /// <summary>
        /// Creates a scriptable object asset
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        public static void CreateScriptableObjectAsset<T>() where T : ScriptableObject
        {
            T newAsset = ScriptableObject.CreateInstance<T>();
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (assetPath == "")
            {
                assetPath = "Assets";
            }
            else if (Path.GetExtension(assetPath) != "")
            {
                assetPath = assetPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string fullAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath + "/New " + typeof(T).ToString() + ".asset");
            AssetDatabase.CreateAsset(newAsset, fullAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }
    }
}
