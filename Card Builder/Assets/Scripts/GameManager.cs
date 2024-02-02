using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] private CardPlayer playerOne;
    [SerializeField] private CardPlayer playerTwo;

    [SerializeField] private GameObject cardObjectPrefab;


    public bool isPlayerOneTurn = true;
    public bool isTakingAction = false;

    public void Start()
    {
        Instance = this;
        Setup();
    }

    public CardPlayer GetOtherPlayer()
    {
        if (isPlayerOneTurn) return playerTwo;
        else return playerOne;
    }

    public void Setup()
    {


        playerOne.Setup(new Vector3(-17, 6, -5.5f), NextTurn);
        playerTwo.Setup(new Vector3(-17, 6, 7.5f), NextTurn);


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            if (isTakingAction)
            {
                Debug.Log("Wait till turn done (Card back in start position)!");
                return;
            }

            isTakingAction = true;
            if (isPlayerOneTurn) playerOne.PlayTurn();
            else playerTwo.PlayTurn();
        }
    }

    public void NextTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
        isTakingAction = false;
    }


}
