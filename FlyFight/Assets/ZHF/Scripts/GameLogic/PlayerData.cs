using UnityEngine;
using System.Collections;

public class PlayerData {
    [HideInInspector]
    public LobbyPlayer lobbyPlayer;
    public string name = "";
    public ShipType type;

    public PlayerData()
    {
        name = NameFactory.RandomGetName();
        type =  (ShipType)Random.Range(0, (int)ShipType.COUNT);

    }
}
