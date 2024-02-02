using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "CardDeck")]
public class CardDeck : ScriptableObject
{

    //Replace with cardData
   // [SerializeField] public List<string> CardDataDeck;
     [SerializeField] public List<CardTemplateCardData> CardDataDeck;
}
