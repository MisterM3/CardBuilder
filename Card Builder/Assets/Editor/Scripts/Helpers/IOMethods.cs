namespace CardBuilder.Helpers
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class IOMethods
    {
        public static Type GetTypeOfObjectUsingObject(UnityEngine.Object findObject)
        {
            string standardAssembly = "Assembly-CSharp";
            return GetTypeOfObjectUsingObject(findObject, standardAssembly);
        }

        public static Type GetTypeOfObjectUsingObject(UnityEngine.Object findObject, string assemblyName)
        {
            string objectPath = AssetDatabase.GetAssetPath(findObject);
            return GetTypeOfObjectUsingPath(objectPath, assemblyName);
        }

        public static Type GetTypeOfObjectUsingPath(string path)
        {
            string standardAssembly = "Assembly-CSharp";
            return GetTypeOfObjectUsingPath(path, standardAssembly);
        }

        public static Type GetTypeOfObjectUsingPath(string path, string assemblyName)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            Type typeObject = Type.GetType($"{fileName}, {assemblyName}");
            return typeObject;
        }

        public static string GetRelativeAssetBasePath(string fullPath)
        {
            if (fullPath == "") return "";
            int index = fullPath.IndexOf("Assets");
            return fullPath.Remove(0, index);
        }

        public static string GetGUIDFromObject(UnityEngine.Object obj)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            return AssetDatabase.AssetPathToGUID(assetPath);
        }

        public static T GetObjectFromGUID<T>(string GUID) where T : UnityEngine.Object
        {
           string assetPath = AssetDatabase.GUIDToAssetPath(GUID);
           return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
    }
}
