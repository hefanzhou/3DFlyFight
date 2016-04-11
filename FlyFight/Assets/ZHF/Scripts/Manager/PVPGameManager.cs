using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.Networking;

public class PVPGameManager : MonoBehaviour {

    [HideInInspector]
    public GamePlayer mineGamePlayer;
    private NetworkConnection conn;
    private static PVPGameManager instance;

    public static PVPGameManager Instance
    {
        get { return PVPGameManager.instance; }
    }
    //[HideInInspector]
    public List<GamePlayer> gamePlayerList;


    void Awake()
    {
        instance = this;
    }
	// Use this for initialization
	void Start () {
        conn = GameLobbyManger.Instance.client.connection;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddPlayer(GamePlayer _gamePlayer)
    {
        if (!gamePlayerList.Contains(_gamePlayer))
        {
            gamePlayerList.Add(_gamePlayer);
        }
    }
}
