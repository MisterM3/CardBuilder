using UnityEngine;

namespace CardBuilder
{
    public class Logs
    {
        public static void Info(string message)
        {
          //  Logs.Info(message);
        }

        public static void Warning(string message)
        {
           // Logs.InfoWarning(message);
        }
        
        public static void Error(string message)
        {
          //  Logs.Error(message);
        }

        public static void NoActiveElementError()
        {
            Logs.Error("No ActiveElement attached to a propertyBox");
        }
    }
}
