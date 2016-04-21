using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour{

    [SyncVar(hook="OnHpChanged")]
    public int hp = 6;
    public int basicHarm = 0;
    [HideInInspector]
    public int killedNum = 0;
    [SyncVar]
    public string playerName;
    [SyncVar]
    public ShipType shipType;

    private ShipCtrl shipCtrl;
    private UIPlayerInfo uiPlayerInfo;
    [ClientCallback]
    void Awake()
    {
        shipCtrl = GetComponent<ShipCtrl>();
        Debug.LogError("Awake" + name + playerName);
    }

    [ClientCallback]
    void Start()
    {
       
        PVPGameManager.Instance.AddPlayer(this);
        if (GetComponent<NetworkIdentity>().hasAuthority)
        {
            PVPGameManager.Instance.mineGamePlayer = this;
        }
        uiPlayerInfo = UIPlayerInfoPanelManager.Instance.AddPlayerInfo();
        uiPlayerInfo.Init(this);
        Debug.LogError("ADDDDDDDD" + name + playerName);
    }
	// Update is called once per frame
	void Update () {
        Debug.LogError("Update" + playerName);
	}
    void OnHpChanged(int newValue)
    {
        hp = newValue;
        uiPlayerInfo.OnHpChange(hp);
    }

    [ServerCallback]
    public void Demage(ShipBullet bullet)
    {
        hp -= (bullet.harm + this.basicHarm);
        if (hp <= 0)
        {
            Killed(bullet.owner);
            bullet.owner.OnKillShip(this);
        }
        uiPlayerInfo.OnHpChange(hp);
    }

    [Server]
    void Killed(GamePlayer killer)
    {
        RpcKilled();
    }

    [ClientRpc]
    void RpcKilled()
    {
        hp = 0;
        SetVisual(false);
    }
    void SetVisual (bool isVisual)
    {
        this.gameObject.SetActive(isVisual);
    }

    [Server]
    void OnKillShip(GamePlayer killedPlayer)
    {
        RpcOnKillShip();
    }

    [ClientRpc]
    void RpcOnKillShip()
    {
        killedNum++;
        uiPlayerInfo.OnKillNumChange(killedNum);
    }

    public void InitByLobbyPlayer(LobbyPlayer lobbyPlayer)
    {
        this.playerName = lobbyPlayer.playerName;
        this.shipType = lobbyPlayer.shipType;
    }

}
