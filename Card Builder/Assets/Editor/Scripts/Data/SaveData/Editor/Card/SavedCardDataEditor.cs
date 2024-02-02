using CardBuilder.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{
    public class SavedCardDataEditor : ScriptableObject
    {
        [SerializeField, HideInInspector] public string nameCard;

        [SerializeField, HideInInspector] public GameObject TemplatePrefab;

        [SerializeField, HideInInspector] public SO_CardData cardDataSO;

        [SerializeField, HideInInspector] public Card card;
    }
}
