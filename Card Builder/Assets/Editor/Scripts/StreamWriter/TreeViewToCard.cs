using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;
using CardBuilder.NewStructs;
using CardBuilder.Helpers;

namespace CardBuilder
{
    public class TreeViewToCard : ScriptableObject
    {

        private static List<string> objects;
        private static List<Object> objectTest;
        private static List<string> connections;

        //EnumConnection starts with name of connected list followed with : and GUID changed with, multiple enums are seperated with ; , Example ("ExampleImage: ImageName; FirstGUID, SecondGUID, ThirdGUID; ExampleString; StringName; FirstString, SecondString, ThirdString")
        private static string enumConnections;

        private static string enumConnectionsStrings;

        private static MonoScript CreateAttachedScript(string name)
        {
            int indexIndent = 0;

            using (StreamWriter sw = new StreamWriter("Assets/SavedData/Canvas/" + name + "Canvas" + ".cs"))
            {
                sw.WriteLineWithIndent("using UnityEngine;", indexIndent);
                sw.WriteLineWithIndent("using System.Collections.Generic;", indexIndent);
                sw.WriteLineWithIndent("using TMPro;", indexIndent);
                sw.WriteLineWithIndent("using UnityEngine.UI;", indexIndent);

                sw.WriteLineWithIndent("", indexIndent);

                sw.WriteLineWithIndent($"public class {name}Canvas : MonoBehaviour, ICardCanvas<{name.FirstCharacterToUpper()}CardData>, IUpdateCard", indexIndent);
                sw.WriteLineWithIndent("{", indexIndent);

                indexIndent++;

                sw.WriteLineWithIndent($"public {name.FirstCharacterToUpper()}CardData cardData" + "{ get; set; }", indexIndent);

                foreach(string objectName in objects)
                {
                    sw.WriteLineWithIndent(objectName, indexIndent);
                }

                sw.WriteLineWithIndent("", indexIndent);

                sw.WriteLineWithIndent($"public void ConnectData({name.FirstCharacterToUpper()}CardData dataToConnect)", indexIndent);

                sw.WriteLineWithIndent("{", indexIndent);

                indexIndent++;

                sw.WriteLineWithIndent($"cardData = dataToConnect;", indexIndent);
                sw.WriteLineWithIndent("UpdateCard();", indexIndent);

                indexIndent--;

                sw.WriteLineWithIndent("}", indexIndent);

                sw.WriteLineWithIndent($"public void UpdateCard()", indexIndent);

                sw.WriteLineWithIndent("{", indexIndent);

                indexIndent++;

                foreach (string objectName in connections)
                {
                    sw.WriteLineWithIndent(objectName, indexIndent);
                }

                indexIndent--;
                sw.WriteLineWithIndent("}", indexIndent);

                indexIndent--;
                sw.WriteLineWithIndent("}", indexIndent);

            }


            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


            EditorPrefs.SetString("ScriptPath", "Assets/SavedData/Canvas/" + name + "Canvas" + ".cs");



            MonoScript mono = AssetDatabase.LoadAssetAtPath("Assets/SavedData/Canvas/" + name + "Canvas" + ".cs", typeof(Object)) as MonoScript;
         

            return mono;


        }


        public static void FinalizeScriptToPrefab()
        {

            string pathScript = EditorPrefs.GetString("ScriptPath");
            string pathPrefab = EditorPrefs.GetString("PrefabPath");
            string pathData = EditorPrefs.GetString("SavedData");

            string enumConnections = EditorPrefs.GetString("SavedEnumConnections");


            EditorPrefs.SetString("ScriptPath", "");
            EditorPrefs.SetString("Saved", "");
            EditorPrefs.SetString("PrefabPath", "");
            EditorPrefs.SetString("SaveWay", "");
            EditorPrefs.SetString("SaveData", "");
            EditorPrefs.SetString("SavedEnumConnections", "");

            GameObject prefab = PrefabUtility.LoadPrefabContents(pathPrefab);

            MonoScript mono = AssetDatabase.LoadAssetAtPath(pathScript, typeof(Object)) as MonoScript;

            Component comp = prefab.AddComponent(mono.GetClass());


          

            foreach (System.Reflection.PropertyInfo info in comp.GetType().GetProperties())
            {
                //First one after self made properties, all properties done, stop
                if (info.Name == "destroyCancellationToken") break;

                if (info.Name == "cardData") info.SetValue(comp, AssetDatabase.LoadAssetAtPath(pathData, info.PropertyType));

                GameObject FoundObject = FindPrefabItem(prefab, info.Name);


                if (FoundObject != null)
                {
                   

                    switch (info.PropertyType.Name)
                    {

                        case "TextMeshProUGUI":
                            TMPro.TextMeshProUGUI newText = FoundObject.GetComponent<TextMeshProUGUI>();
                            info.SetValue(comp, newText);
                            break;
                        case "Image":
                            UnityEngine.UI.Image newImage = FoundObject.GetComponent<UnityEngine.UI.Image>();
                            info.SetValue(comp, newImage);
                            break;
                        case "List<Sprite>":
                            Logs.Info(info.Name);
                            break;
                    }


                }

            }

           


            Dictionary<string, List<Sprite>> connected = ConnectDictionary(enumConnections);

            Dictionary<string, List<string>> connectedString = ConnectStringDictionary(enumConnectionsStrings);





            foreach (System.Reflection.FieldInfo fieldInfo in comp.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
            {
                foreach(KeyValuePair<string, List<Sprite>> kvp in connected)
                {
                    if (kvp.Key != fieldInfo.Name) continue;

                    fieldInfo.SetValue(comp, kvp.Value);
                    continue;
                }

                foreach(KeyValuePair<string, List<string>> kvp in connectedString)
                {
                    if (kvp.Key != fieldInfo.Name) continue;

                    fieldInfo.SetValue(comp, kvp.Value);
                    continue;
                }
            }


            
           

            PrefabUtility.SaveAsPrefabAsset(prefab, pathPrefab, out bool succes);

            PrefabUtility.UnloadPrefabContents(prefab);

            AssetDatabase.Refresh();

            return;

        }

        public static Dictionary<string, List<Sprite>> ConnectDictionary(string fullString)
        {
            Dictionary<string, List<Sprite>> tempDictionary = new();

            List<string> seperateConnections = new List<string>();

            string[] sepCon = fullString.Split(";");

            foreach (string str in sepCon)
            {
                if (str == "") continue;
                string[] splitSec = str.Split(":");

                string[] differentGUID = splitSec[1].Split(",");

                List<Sprite> spriteList = new();

                foreach(string GUID in differentGUID)
                {

                    if (GUID == "") continue;
                    Sprite sprite = IOMethods.GetObjectFromGUID<Sprite>(GUID);
                    spriteList.Add(sprite);
                }

                tempDictionary.Add(splitSec[0], spriteList);
            }

            return tempDictionary;
        }

        public static Dictionary<string, List<string>> ConnectStringDictionary(string fullString)
        {
            Dictionary<string, List<string>> tempDictionary = new();

            List<string> seperateConnections = new List<string>();


            if (string.IsNullOrEmpty(fullString)) return tempDictionary;

            string[] sepCon = fullString.Split(";");

            foreach (string str in sepCon)
            {
                if (str == "") continue;
                string[] splitSec = str.Split(":");

                string[] differentStrings = splitSec[1].Split(",");

                List<string> stringList = new();

                foreach (string strings in differentStrings)
                {
                    stringList.Add(strings);
                }

                tempDictionary.Add(splitSec[0], stringList);
            }

            return tempDictionary;
        }

        public static GameObject FindPrefabItem(GameObject baseObject, string nameToFind)
        {
            if (baseObject.name == nameToFind) return baseObject;



            
            for(int i = 0; i < baseObject.transform.childCount; i++)
            {
                Transform child = baseObject.transform.GetChild(i);
                GameObject hasObject = FindPrefabItem(child.gameObject, nameToFind);

                if (hasObject != null) return hasObject;

            }
            

            return null;
        }

        public static void CreatePrefabFromTreeView(TreeViewItemData<HierarchyData> rootItem, string name)
        {
            objects = new();
            connections = new();
            objectTest = new();

            enumConnections = "";


            string pathPrefab = $"Assets/SavedInfo/Prefabs/{name}Prefab.prefab";

            GameObject newObj = new();
            newObj.name = $"{name}Prefab";

            Canvas newCanvas = newObj.AddComponent<Canvas>();
            newCanvas.renderMode = RenderMode.WorldSpace;


            if (rootItem.hasChildren) 
            MakeChildren(rootItem, newObj);

            MonoScript script = CreateAttachedScript(name);



        //    GameObject prefab = PrefabUtility.LoadPrefabContents(pathPrefab);



            EditorPrefs.SetString("SaveWay", "SaveTemplate");
            EditorPrefs.SetString("SavedEnumConnections", enumConnections);
            EditorPrefs.SetString("SavedEnumConnectionsStrings", enumConnectionsStrings);
            EditorPrefs.SetString("PrefabPath", pathPrefab);

            PrefabUtility.SaveAsPrefabAsset(newObj, pathPrefab, out bool succes);

            EditorUtility.SetDirty(newObj);

            DestroyImmediate(newObj);
            AssetDatabase.Refresh();

          //  Logs.Info(script.GetClass());



            if (script.GetClass() == null) return;

           FinalizeScriptToPrefab();



        }



        private static void MakeChildren(TreeViewItemData<HierarchyData> rootItem, GameObject parentObject)
        {
            foreach (TreeViewItemData<HierarchyData> child in rootItem.children)
            {

                GameObject newObject = null;

                if (child.data.VisualElement is UndoRedoImage undoRedoImage)
                {
                    newObject = CreateImageFromUndoRedoImage(undoRedoImage, child.data.Name);
                   
                }

                if (child.data.VisualElement is UndoRedoText undoRedoText)
                {
                    newObject = CreateTextFromUndoRedoText(undoRedoText, child.data.Name);
                }

                if (newObject == null) continue;

                newObject.name = StreamWriterMethods.ConvertPropertyToLine(child.data.Name);

                Vector2 position = newObject.GetComponent<RectTransform>().localPosition;
                newObject.GetComponent<RectTransform>().SetParent(parentObject.transform);
                newObject.GetComponent<RectTransform>().localPosition = position;


                if (rootItem.hasChildren) MakeChildren(child, newObject);
            }
        }



        private static GameObject CreateImageFromUndoRedoImage(UndoRedoImage undoRedoImage, string Name)
        {
            GameObject newObj = new();
            UnityEngine.UI.Image newImage = newObj.AddComponent<UnityEngine.UI.Image>();

            newImage.sprite = undoRedoImage.Image;



            newObj.GetComponent<RectTransform>().localPosition = new Vector2(undoRedoImage.Position.x, -undoRedoImage.Position.y);
            newObj.GetComponent<RectTransform>().sizeDelta = undoRedoImage.Size;


            if (undoRedoImage.ConnectedInfo != null)
            {
                objects.Add($"[field: SerializeField] public Image {StreamWriterMethods.ConvertPropertyToLine(Name)}" + "{ get; set; }");
                objectTest.Add(newImage);

                if (undoRedoImage.ConnectedInfo.PropertyType == PropertyType.Enum)
                {
                    objects.Add($"[SerializeField] private List<Sprite> {StreamWriterMethods.ConvertToVariable(Name)}List;");
                    objects.Add($"public List<Sprite> {StreamWriterMethods.ConvertPropertyToLine(Name)}List" + " {" + $"get => {StreamWriterMethods.ConvertToVariable(Name)}List; set => {StreamWriterMethods.ConvertToVariable(Name)}List = value;"  + " }");
                    connections.Add($"{StreamWriterMethods.ConvertPropertyToLine(Name)}.sprite = {StreamWriterMethods.ConvertPropertyToLine(Name)}List[(int)cardData.{StreamWriterMethods.ConvertPropertyToLine(undoRedoImage.ConnectedInfo.NameProperty)}];");

                    enumConnections += MakeConnectedStringFromImageEnum(undoRedoImage, Name);
                }
                else if (undoRedoImage.ConnectedInfo.PropertyType == PropertyType.Sprite)
                    connections.Add($"{StreamWriterMethods.ConvertPropertyToLine(Name)}.sprite = cardData.{StreamWriterMethods.ConvertPropertyToLine(undoRedoImage.ConnectedInfo.NameProperty)};");
            }


            return newObj;
        }

        private static string MakeConnectedStringFromImageEnum(UndoRedoImage image, string Name)
        {
            string saveList = $"{StreamWriterMethods.ConvertToVariable(Name)}List:";
            
            foreach(Sprite sprite in image.SpriteList)
            {
                string guid = IOMethods.GetGUIDFromObject(sprite);

                saveList += $"{guid},";
            }

            saveList += ";";

            return saveList;
        }

        private static string MakeConnectedStringFromStringEnum(UndoRedoText text, string Name)
        {
            string saveList = $"{StreamWriterMethods.ConvertToVariable(Name)}List:";

            foreach (string strings in text.TextList)
            {

                saveList += $"{strings},";
            }

            saveList += ";";

            return saveList;
        }

        private static GameObject CreateTextFromUndoRedoText(UndoRedoText undoRedoText, string Name)
        {
            GameObject newObj = new();
            TextMeshProUGUI newText = newObj.AddComponent<TextMeshProUGUI>();

            Debug.Log("____________");
            Debug.Log(Name);
            Debug.Log(undoRedoText.Position);
            Debug.Log(undoRedoText.Font);
            Debug.Log("____________");

            newText.text = undoRedoText.Text;
            if (undoRedoText.Font != null)
            {


                newText.font = undoRedoText.Font;
                newText.UpdateFontAsset();

            }
            newText.fontSize = undoRedoText.FontSize;
            newText.color = undoRedoText.TextColour;

            newText.alignment = undoRedoText.TMP_Alignment;

            newObj.GetComponent<RectTransform>().localPosition = new Vector2(undoRedoText.Position.x, -undoRedoText.Position.y);
            newObj.GetComponent<RectTransform>().sizeDelta = undoRedoText.Size;

            if (undoRedoText.ConnectedInfo != null)
            {

                if (undoRedoText.ConnectedInfo.NameProperty == "") return newObj;
            //    Logs.Info("dea: " + undoRedoText.ConnectedInfo.NameProperty);

                objects.Add($"[field: SerializeField] public TextMeshProUGUI {StreamWriterMethods.ConvertPropertyToLine(Name)}" + "{ get; set; }");
                objectTest.Add(newText);

                if (undoRedoText.ConnectedInfo.PropertyType == PropertyType.Enum)
                {
                    objects.Add($"[SerializeField] private List<string> {StreamWriterMethods.ConvertToVariable(Name)}List;");
                    objects.Add($"public List<string> {StreamWriterMethods.ConvertPropertyToLine(Name)}List" + " {" + $"get => {StreamWriterMethods.ConvertToVariable(Name)}List; set => {StreamWriterMethods.ConvertToVariable(Name)}List = value;" + " }");
                    connections.Add($"{StreamWriterMethods.ConvertPropertyToLine(Name)}.text = {StreamWriterMethods.ConvertPropertyToLine(Name)}List[(int)cardData.{StreamWriterMethods.ConvertPropertyToLine(undoRedoText.ConnectedInfo.NameProperty)}];");

                    enumConnectionsStrings += MakeConnectedStringFromStringEnum(undoRedoText, Name);
                }
                else if (undoRedoText.ConnectedInfo.PropertyType == PropertyType.String)
                    connections.Add($"{StreamWriterMethods.ConvertPropertyToLine(Name)}.text = cardData.{StreamWriterMethods.ConvertPropertyToLine(undoRedoText.ConnectedInfo.NameProperty)};");
                else if (undoRedoText.ConnectedInfo.PropertyType == PropertyType.Integer)
                    connections.Add($"{StreamWriterMethods.ConvertPropertyToLine(Name)}.text = cardData.{StreamWriterMethods.ConvertPropertyToLine(undoRedoText.ConnectedInfo.NameProperty)}.ToString();");
            }

                     

            return newObj;
        }






    }
}
