namespace CardBuilder.Helpers
{
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.VisualScripting;
    public static class StreamWriterMethods
    {
        public static void WriteLineWithIndent(this StreamWriter sr, string text, int index = 0)
        {
            text = text.Indent(index);
            sr.WriteLine(text);
        }



        public static string Indent(this string stringToChange, int index = 0)
        {
            for (int i = 0; i < index; i++)
            {
                stringToChange = stringToChange.Insert(0, "    ");
            }
            return stringToChange;
        }

        public static string ConvertPropertyToLine(string startingString)
        {
            startingString = startingString.TrimEnd();
            startingString = startingString.TrimStart();
            startingString = startingString.ToLower();

            startingString = ToUpperAfterEachSpace(startingString);

            startingString = startingString.Replace('(', '_');
            startingString = startingString.Replace(')', '_');
            startingString = startingString.Trim();

            return startingString;
        }

        public static string ConvertToVariable(string startingString)
        {
            startingString = ConvertPropertyToLine(startingString);

            //V for value
            return startingString.Insert(0, "v_");
        }

        public static string ToUpperAfterEachSpace(string text)
        {
            IEnumerable<int> indexList = text.AllIndexesOf(" ");

            string newText = "";

            if (indexList.Count() == 0) return text;

            int lastIndex = -2;

            foreach (int index in indexList.ToList())
            {
                newText += text.Substring(lastIndex + 2, index - lastIndex - 2);
                string textPart = text.Substring(index, 2);
                newText += textPart.ToUpper().Trim();

                lastIndex = index;
            }

            newText += text.Substring(lastIndex + 2, text.Length - (lastIndex + 2));

            return newText;
        }
    }
}
