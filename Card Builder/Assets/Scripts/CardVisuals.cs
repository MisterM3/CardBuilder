using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardObject))]
public class CardVisuals : MonoBehaviour
{

    Action onMoveCompleted;
    private Action<CardObject> onCardAttack;
    CardObject cardToAttack;

    [SerializeField] AnimationCurve yCurve;
    [SerializeField] AnimationCurve horizontalSpeed;

    [SerializeField] GameObject ParticleObject;

    Vector3 targetPosition;
    Vector3 startPosition;

    //Seconds for attack
    float timeToAttack = .75f;
    public void Setup(Action onMoveCompleted)
    {

    }

    public void MoveToPosition(Vector3 positionMoveTo, CardObject cardToAtttack, Action<CardObject> onCardAttack, Action onMoveCompleted)
    {
        this.onMoveCompleted = onMoveCompleted;
        this.onCardAttack = onCardAttack;
        this.cardToAttack = cardToAtttack;
        targetPosition = positionMoveTo;
        startPosition = this.transform.position;


        StartCoroutine("MovePosition");

       // onMoveCompleted(cardToAtttack);
    }

    public IEnumerator MovePosition()
    {
        bool toCard = true;

        float time = 0;

        Vector3 distanceToGo = targetPosition - this.transform.position;

        while (toCard)
        {

            time += Time.deltaTime;

            

            Vector3 direction = (targetPosition - this.transform.position).normalized;

            float yPos = startPosition.y + yCurve.Evaluate(time);

            this.transform.position = targetPosition * horizontalSpeed.Evaluate(time) + startPosition * (1 - horizontalSpeed.Evaluate(time));
            this.transform.position = new Vector3(this.transform.position.x, yPos, this.transform.position.z);

            if (time >= timeToAttack * 0.9)
            {
                Instantiate(ParticleObject, targetPosition + new Vector3(0, .1f, 0), Quaternion.LookRotation(distanceToGo));
            }
            if (time >= timeToAttack)
            {
                toCard = false;
                this.transform.position = targetPosition;
                onCardAttack(cardToAttack);


                cardToAttack = null;
            }
            
            yield return 0;

        }

        bool toStartPosition = true;

        time = 0;

        while (toStartPosition)
        {
            time += Time.deltaTime;

            this.transform.position = startPosition * horizontalSpeed.Evaluate(time) + targetPosition * (1 - horizontalSpeed.Evaluate(time));

            if (Vector3.Distance(this.transform.position, startPosition) < .5f || time >= 1)
            {
                toStartPosition = false;
                this.transform.position = startPosition;
            }

            yield return 0;

        }

        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
        onMoveCompleted();

    }

}
