using UnityEngine;
using System.Collections;

public class Player {
    [HideInInspector]
    public LobbyPlayer lobbyPlayer;
    public string name = "";
    public ShipType type;

    public Player()
    {
        name = NameFactory.RandomGetName();
        type =  (ShipType)Random.Range(0, (int)ShipType.COUNT);

    }
}
