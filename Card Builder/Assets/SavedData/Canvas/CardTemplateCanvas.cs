using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CardTemplateCanvas : MonoBehaviour, ICardCanvas<CardTemplateCardData>, IUpdateCard
{
    public CardTemplateCardData cardData{ get; set; }
    [field: SerializeField] public Image background{ get; set; }
    [SerializeField] private List<Sprite> v_backgroundList;
    public List<Sprite> backgroundList {get => v_backgroundList; set => v_backgroundList = value; }
    [field: SerializeField] public Image cardNameImage{ get; set; }
    [field: SerializeField] public TextMeshProUGUI cardName{ get; set; }
    [field: SerializeField] public Image borderImage{ get; set; }
    [field: SerializeField] public Image charachterImage{ get; set; }
    [field: SerializeField] public Image healthImage{ get; set; }
    [field: SerializeField] public TextMeshProUGUI healthText{ get; set; }
    [field: SerializeField] public Image damageImage{ get; set; }
    [field: SerializeField] public TextMeshProUGUI damageText{ get; set; }
    
    public void ConnectData(CardTemplateCardData dataToConnect)
    {
        cardData = dataToConnect;
        UpdateCard();
    }
    public void UpdateCard()
    {
        background.sprite = backgroundList[(int)cardData.rarity];
        cardName.text = cardData.cardName;
        charachterImage.sprite = cardData.charachterImage;
        healthText.text = cardData.health.ToString();
        damageText.text = cardData.damage.ToString();
    }
}
