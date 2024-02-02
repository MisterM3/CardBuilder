using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{

    [SerializeField] private GameObject CardVisualsPrefab;

    //Replace with cardData
    //  [SerializeField] private string CardData;

    [SerializeField] private CardTemplateCardData CardData;

    private CardTemplateCanvas canvas;

    CardVisuals cardVisuals;

    public EventHandler onCardDestroyed;

    Action onActionDone;


   // public void Setup(Action onActionDone, string data)
    public void Setup(Action onActionDone, CardTemplateCardData data)
    {
        CardData = data;
        cardVisuals = this.gameObject.GetComponent<CardVisuals>();

        this.onActionDone = onActionDone;
        GameObject cardVisual = Instantiate(CardVisualsPrefab, this.transform);


        
        cardVisual.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        cardVisual.transform.rotation = Quaternion.Euler(90, 0, 0);
        canvas = cardVisual.GetComponent<CardTemplateCanvas>();
        canvas.ConnectData(CardData);
        

    }


    public void Action(CardObject cardToAttack)
    {
        cardVisuals.MoveToPosition(cardToAttack.transform.position, cardToAttack, DamageEnemy, onActionDone);
    }

    public void DamageEnemy(CardObject cardToAttack)
    {
       // cardToAttack.TakeDamage(1);
        cardToAttack.TakeDamage(CardData.damage);
    }

    public void TakeDamage(int damage)
    {
      //  DestroyCard();

        
        CardData.health -= damage;

        canvas.UpdateCard();

        if (CardData.health <= 0)
            DestroyCard();
        
    }

    public void DestroyCard()
    {
        onCardDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(this.gameObject);
    }
}
