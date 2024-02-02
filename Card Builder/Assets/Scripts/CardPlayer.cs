using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayer : MonoBehaviour
{

    [SerializeField] public CardDeck deck;

    public List<CardObject> cardsOnTable;

  //  [SerializeField] public List<Card> cardsOnTable;

    [SerializeField] private GameObject cardObjectPrefab;


    public void Setup(Vector3 position, Action onTurnCompleted)
    {

        Vector3 positionDelta = new Vector3(10f, 0, -0);

        for (int i = 0; i < 4; i++)
        {
            Vector3 newPosition = position + positionDelta * i;
            GameObject obj = Instantiate(cardObjectPrefab, newPosition, Quaternion.identity);

            
            
            CardObject card = obj.GetComponent<CardObject>();

          //  card.Setup(onTurnCompleted, i.ToString());
            card.Setup(onTurnCompleted, Instantiate(deck.CardDataDeck[i]));
            
            card.onCardDestroyed += Card_OnCardDestroyed;
            cardsOnTable.Add(card);
            
        }


    }

    private void Card_OnCardDestroyed(object sender, EventArgs e)
    {
        if (sender is CardObject card)
        {
            cardsOnTable.Remove(card);
        }
    }

    public void PlayTurn()
    {
        CardObject cardToPlay = GetRandomCard();

        if (cardToPlay == null) return;

        CardPlayer enemy = GameManager.Instance.GetOtherPlayer();
        CardObject cardToAttack = enemy.GetRandomCard();

        cardToPlay.Action(cardToAttack);

    }


    public CardObject GetRandomCard()
    {
        if (cardsOnTable.Count == 0)
        {
            Debug.Log("No more cards on this player");
            return null;
        }

        int randomCardIndex = UnityEngine.Random.Range(0, cardsOnTable.Count);

        return cardsOnTable[randomCardIndex];
    }

}
