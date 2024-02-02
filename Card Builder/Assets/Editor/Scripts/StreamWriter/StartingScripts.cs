using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using CardBuilder.Data;
using System;
using Unity.VisualScripting;
using CardBuilder.Helpers;

namespace CardBuilder
{
    public static class StartingScripts 
    {
        

        public static string SaveLoadMainFile(string path)
        {

            int lastIindex = path.LastIndexOf('/');

            string lastPart = path.Substring(lastIindex + 1);

            lastPart = lastPart.Remove(lastPart.IndexOf('.'));

            Logs.Info(lastPart);

            TemplateData newDataTemplate = ScriptableObject.CreateInstance<TemplateData>();


            SO_CardData data = TryCreateCardDataSO(lastPart);
            

            newDataTemplate.cardDataSO = data;
            newDataTemplate.templateName = lastPart;
            newDataTemplate.hierarchyData = new();

            int savePathIndex = path.IndexOf("Assets");
            string savePath = path.Remove(0, savePathIndex);

            Logs.Info(savePath);   


            AssetDatabase.CreateAsset(newDataTemplate, savePath);

        //    CreateDataScript(lastPart);
         //   CreatePrefabScript(lastPart);
        //    CreatePrefab(lastPart);


            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


            return savePath;
        }

        private static void CreatePrefab(string name)
        {
            string pathPrefab = $"Assets/SavedInfo/Prefabs/{name}Prefab.prefab";

            GameObject newObj = new();
            newObj.name = $"{name}Prefab";
            EditorPrefs.SetString("PrefabPath", pathPrefab);

            PrefabUtility.SaveAsPrefabAsset(newObj, pathPrefab, out bool succes);

            EditorUtility.SetDirty(newObj);
           // UnityEngine.Object.DestroyImmediate(newObj);

        }

        private static void CreateDataScript(string name)
        {
            int intentIndex = 0;

            using (StreamWriter sw = new StreamWriter($"Assets/SavedInfo/Data/{name.FirstCharacterToUpper()}CardData.cs"))
            {

                sw.WriteLineWithIndent("using System;", intentIndex);
                sw.WriteLineWithIndent("using UnityEngine;", intentIndex);

                sw.WriteLine("");

                sw.WriteLineWithIndent("[Serializable]", intentIndex);
                sw.WriteLineWithIndent($"public class {name.FirstCharacterToUpper()}CardData : Card", intentIndex);


                sw.WriteLineWithIndent("{", intentIndex);

                sw.WriteLineWithIndent("}", intentIndex);

            }

            EditorPrefs.SetString("SavedData", $"Assets/SavedInfo/Data/{name.FirstCharacterToUpper()}CardData.cs");
           // AssetDatabase.SaveAssets();
        }

        private static void CreatePrefabScript(string name)
        {

            int indexIndent = 0;
            using (StreamWriter sw = new StreamWriter("Assets/SavedData/Canvas/" + name + "Canvas" + ".cs"))
            {
                sw.WriteLineWithIndent("using UnityEngine;", indexIndent);

                sw.WriteLineWithIndent("", indexIndent);

                sw.WriteLineWithIndent($"public class {name}Canvas : MonoBehaviour");
                sw.WriteLineWithIndent("{", indexIndent);
                sw.WriteLineWithIndent("}", indexIndent);
            }

            EditorPrefs.SetString("ScriptPath", "Assets/SavedData/Canvas/" + name + "Canvas" + ".cs");
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        }
    
    

        public static bool StartingDataSaveLoading(string name)
        {
            TryCreateCardDataSO(name);
         //   TryCreateTemplateSavingSO(name);
            return true;
        }

        public static bool TryCreateStartingScripts(string name)
        {

            TryCreateStartCardData(name);

            TryCreateStartPrefab(name);
            TryCreateStartPrefabData(name);

            return true;
        }

        private static bool TryCreateStartPrefabData(string name)
        {
            StreamWriter sw;
            if (!File.Exists($"Assets/Editor/SavedObjects/Temporary/Data/{name}Data.cs"))
            {
                sw = File.CreateText($"Assets/Editor/SavedObjects/Temporary/Data/{name}Data.cs");
            }
            else
            {
                sw = new StreamWriter($"Assets/Editor/SavedObjects/Temporary/Data/{name}Data.cs");
            }


                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("using CardBuilder;");

                sw.WriteLine("");

            sw.Flush();
            sw.Close();
            

            AssetDatabase.SaveAssets();

            return true;
        }

        private static bool TryCreateStartPrefab(string name)
        {
            GameObject newObj = new();

            Canvas newCanvas = newObj.AddComponent<Canvas>();
            newCanvas.renderMode = RenderMode.WorldSpace;

            PrefabUtility.SaveAsPrefabAsset(newObj, $"Assets/Editor/SavedObjects/Temporary/Prefabs/{name}Prefab.prefab", out bool succes);


            return succes;
        }

        private static bool TryCreateStartCardData(string name)
        {
            TemplateData data = ScriptableObject.CreateInstance<TemplateData>();

          //  AssetDatabase.CreateAsset(data, $"Assets/Editor/SavedObjects/Temporary/Data/NewTemplate/{name}TemplateData.cs");

            return true;
        }


        private static SO_CardData TryCreateCardDataSO(string name)
        {
            SO_CardData data = ScriptableObject.CreateInstance<SO_CardData>();

            AssetDatabase.CreateAsset(data, $"Assets/Editor/SavedDataLoading/{name}CardDataLoading.asset");

            return data;
        }


    }
}
