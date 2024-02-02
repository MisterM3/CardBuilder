using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICardCanvas<TCardData>
{
    public TCardData cardData { get; set; }

    public void ConnectData(TCardData dataToConnect);


}

public interface IUpdateCard
{
    public void UpdateCard();
}

