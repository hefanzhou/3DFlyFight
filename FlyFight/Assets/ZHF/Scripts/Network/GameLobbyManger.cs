using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class GameLobbyManger : NetworkLobbyManager
{
    private static GameLobbyManger instance = null;
    public static GameLobbyManger Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    public void OnMatchCreate(CreateMatchResponse matchInfo)
    {
        base.OnMatchCreate(matchInfo);

    }

    public void CreateMatch(string name, uint size, string password = "")
    {
        StartMatchMaker();
        matchMaker.CreateMatch(name, size, true, password, OnMatchCreate);
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("@@@OnLobbyServerCreateLobbyPlayer");
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;
        UpdateMinPlayers();
        ++minPlayers;//current not create lobbyplayer but updatefunction relay it
        return obj;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("@@@@OnClientConnect");
        base.OnClientConnect(conn);
    }


    public override void OnStartHost()
    {
        Debug.LogError("@@@@OnStartHost");
        base.OnStartHost();
    }

    public void RequsetPage(int page, NetworkMatch.ResponseDelegate<ListMatchResponse> callback, string roomName = "")
    {
        //只是在做赋值host port工作 所以可重复调用
        GameLobbyManger.Instance.StartMatchMaker();
        matchMaker.ListMatches(page, 6, roomName, callback);
    }

    public void JoinMatch(NetworkID id, string password = "")
    {
        matchMaker.JoinMatch(id, password, OnMatchJoined);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.LogError("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()));
    }

    public void DirectJoin(string ipAddress = "127.0.0.1")
    {
        networkAddress = ipAddress;
        StartClient();
    }

    public override void OnLobbyServerPlayersReady()
    {
        Debug.LogError("@@@@@OnLobbyServerPlayersReady");
    }


    //本地调用 服务器不会因为多个客户端进入调用多次
    public override void OnLobbyClientEnter()
    {
        Debug.LogError("@@@@OnLobbyClientEnter");
        base.OnLobbyClientEnter();

    }

    public override void OnLobbyClientExit()
    {
        base.OnLobbyClientExit();
    }

    public void UpdateMinPlayers()
    {
        int result = 0;
        foreach (var temp in lobbySlots)
        {
            if (temp != null) ++result;
        }
        minPlayers = result;
        Debug.Log(minPlayers + "@@" + result);
    }
}


