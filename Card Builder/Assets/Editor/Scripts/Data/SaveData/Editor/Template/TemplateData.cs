using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{

    using CardBuilder.Data;
    using UnityEngine.UIElements;

    public class TemplateData : ScriptableObject
    {
        
        [SerializeField, HideInInspector] public string templateName;

        [SerializeField, HideInInspector] public SO_CardData cardDataSO;

        [SerializeField, HideInInspector] public SavingHierarchyList hierarchyData;

    }
}
