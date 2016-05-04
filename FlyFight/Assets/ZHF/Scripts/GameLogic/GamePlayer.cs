using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class GamePlayer : NetworkBehaviour, IComparable<GamePlayer>
{

    [SyncVar(hook="OnHpChanged")]
    public int hp = 6;
     [SyncVar]
    public string playerName;
    [SyncVar]
    public int basicHarm = 0;

    [HideInInspector]
    [SyncVar]
    public ShipType shipType;

    public Action<int> OnKillePlayerAmountEvent;
    public Action<int> OnDeathAmountEvent;
    public Action<int> OnHpChangeEvent;

    private int killePlayerAmount = 0;

    public int KillePlayerAmount
    {
        get { return killePlayerAmount; }
        set
        {
            killePlayerAmount = value;
            if(OnKillePlayerAmountEvent != null) OnKillePlayerAmountEvent(killePlayerAmount);
        }
    }
    private int deathAmount = 0;

    public int DeathAmount
    {
        get { return deathAmount; }
        set 
        {
            deathAmount = value;
            if(OnDeathAmountEvent != null) OnDeathAmountEvent(deathAmount);
        }
    }
    [HideInInspector]
    public bool canCtl = true;

    private bool isDeath = false;

    public bool IsDeath
    {
        get { return isDeath; }
    }

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
    }

    void OnHpChanged(int newValue)
    {
        hp = newValue;
        if (OnHpChangeEvent != null) OnHpChangeEvent(newValue);
    }


    [Server]
    public void Killed(GamePlayer killer)
    {
        RpcKilled();
    }

    [ClientRpc]
    void RpcKilled()
    {
        hp = 0;
        DeathAmount++;
        ProcessDeath();

    }
    void ProcessDeath ()
    {
        isDeath = true;
        this.gameObject.SetActive(false);
        if(hasAuthority && !PVPGameManager.Instance.IsGameOver)
        {
            CountDownManger.Instance.ShowCountDown(5, "{0:0.0} scends rebirth..", () => { CmdRebirth(); }, 0.1f);
        }
    }
    void OnGUI()
    {
        if (!hasAuthority) return;
        //if (GUILayout.Button("Rebirth")) CmdRebirth();

    }
    [Command]
    void CmdRebirth()
    {
        RpcRebirth();
    }

    [ClientRpc]
    void RpcRebirth()
    {
        isDeath = false;
        hp = 6;
        //uiPlayerInfo.OnHpChange(hp);
        this.transform.position =  GameLobbyManger.Instance.GetStartPosition().position;
        this.gameObject.SetActive(true);
    }

    [Server]
    public void OnKillShip(GamePlayer killedPlayer)
    {
        RpcOnKillShip();
    }

    [ClientRpc]
    void RpcOnKillShip()
    {
        KillePlayerAmount++;
        //uiPlayerInfo.OnKillNumChange(killePlayerAmount);
    }

    public void InitByLobbyPlayer(LobbyPlayer lobbyPlayer)
    {
        this.playerName = lobbyPlayer.playerName;
        this.shipType = lobbyPlayer.shipType;
    }






    int IComparable<GamePlayer>.CompareTo(GamePlayer other)
    {
        if (this.killePlayerAmount == other.killePlayerAmount) return other.deathAmount - this.deathAmount;
        else return other.killePlayerAmount - this.killePlayerAmount;
    }
}
