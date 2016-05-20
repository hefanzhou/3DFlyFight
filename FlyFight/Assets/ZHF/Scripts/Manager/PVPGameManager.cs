using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.Networking;

public class PVPGameManager : NetworkBehaviour {

    //kill times for game over
    public int MaxKillAmount  = 3;
    [HideInInspector]
    public GamePlayer mineGamePlayer;

    private bool isGameOver = false;

    public bool IsGameOver
    {
        get { return isGameOver; }
    }
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
    [Server]
    void CheckIsGameOver(int killePlayerAmount)
    {
        if (killePlayerAmount >= MaxKillAmount)
        {
            RpcProcessGameOver();
            isGameOver = true;
        }
    }


    [ClientRpc]
    void RpcProcessGameOver()
    {
        Debug.LogError("RpcProcessGameOver");
        isGameOver = true;
        UIPlayerInfoPanelManager.Instance.ClosePanle();
        UICountPanelManager.Instance.OpenPanel();
    }
    public void AddPlayer(GamePlayer _gamePlayer)
    {
        if (!gamePlayerList.Contains(_gamePlayer))
        {
            gamePlayerList.Add(_gamePlayer);
        }
    }

    [ServerCallback]
    public void CalcDemage(GamePlayer demagePlayer, ShipBullet bullet)
    {
        demagePlayer.hp -= (bullet.harm + demagePlayer.basicHarm);
        if (demagePlayer.hp <= 0)
        {
            CheckIsGameOver(bullet.owner.KillePlayerAmount + 1);
            demagePlayer.Killed(bullet.owner);
            bullet.owner.OnKillShip(demagePlayer);
        }
    }

  
}
