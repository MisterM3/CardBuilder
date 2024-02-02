namespace CardBuilder
{
    using UnityEngine;
    using System.IO;
    using UnityEditor;
    using CardBuilder.Data;
    using System;
    using Unity.VisualScripting;
    using CardBuilder.Helpers;
    using System.Collections.Generic;

    public static class SaveTemplateDataToSO
    {

        public static void MakeFile(string name, SO_CardData data)
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

                intentIndex++;





                foreach (IntProperty intProperty in data.IntPropertyList)
                {
                    sw.WriteLineWithIndent(LineFromProperty(intProperty), intentIndex);
                }
                    sw.WriteLine("");

                foreach (StringProperty stringProperty in data.StringPropertyList)
                {
                    sw.WriteLineWithIndent(LineFromProperty(stringProperty), intentIndex);
                }
                    sw.WriteLine("");

                foreach (SpriteProperty spriteProperty in data.SpritePropertyList)
                {
                    sw.WriteLineWithIndent(LineFromProperty(spriteProperty), intentIndex);
                }
                    sw.WriteLine("");

                foreach (EnumProperty enumProperty in data.EnumPropertyList)
                {
                    sw.WriteLineWithIndent(LineFromProperty(enumProperty), intentIndex);
                }
                sw.WriteLine("");

                intentIndex--;

                sw.WriteLineWithIndent("}", intentIndex);
               
            }

            EditorPrefs.SetString("SavedData", $"Assets/SavedInfo/Data/{name.FirstCharacterToUpper()}CardData.cs");
            AssetDatabase.SaveAssets();
        }


        private static string LineFromProperty<T>(Properties<T> property)
        {
            string prefix = "public";

            string variableName = property.PropertyLabel;
            variableName = StreamWriterMethods.ConvertPropertyToLine(variableName);

            string type = "";


            switch(property.Value)
            {
                case MonoScript monoScript:
                    Type getType = IOMethods.GetTypeOfObjectUsingObject(monoScript);
                    type = getType.Name;
                    break;
                case System.Int32:
                    type = "int";
                    break;
                case System.String:
                    type = "string";
                    break;
                //Sprite returns only one as null
                default:
                    type = "Sprite";
                    break;
            }

            return $"{prefix} {type} {variableName};";

        }
    }
}
