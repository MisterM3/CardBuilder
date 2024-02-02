using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{
    public class ActiveElementPanelData : ScriptableObject
    {

        static List<PropertyInfo> allProperties = new();

        public static EventHandler OnListChanged;



        public void Init()
        {
            allProperties = new();
        }

        public void AddProperty(PropertyInfo item)
        {
            allProperties.Add(item);
            OnListChanged?.Invoke(null, EventArgs.Empty);
        }

        public void RemoveProperty(PropertyInfo item)
        {
            if (allProperties.Contains(item))
            allProperties.Remove(item);
            OnListChanged?.Invoke(null, EventArgs.Empty);
        }

        public static List<PropertyInfo> GetList()
        {
            return allProperties;
        }
    }
}
