using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

public class GameLobbyManger : NetworkLobbyManager
{

    [HideInInspector]
    public PlayerData mainPlayerData;

    public GameObject[] playerPerfabs;
    private static GameLobbyManger instance = null;
    public static GameLobbyManger Instance
    {
        get
        {
            return instance;
        }
    }

    private bool isServer = false;

    public bool IsServer
    {
        get { return isServer; }
    }


    void Awake()
    {
        instance = this;
        mainPlayerData = new PlayerData();
        Initialize();
    }

    void Initialize()
    {
        foreach(var go in playerPerfabs)
        {
            ClientScene.RegisterPrefab(go);
        }
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
        Debug.LogError("@@@OnLobbyServerCreateLobbyPlayer");
        foreach(var item in conn.playerControllers)
        {
            Debug.LogError(item.gameObject.name + item.playerControllerId);
        }
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;
        UpdateMinPlayers();
        ++minPlayers;//current not create lobbyplayer but updatefunction relay it
        return obj;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("@@@@OnClientConnect");
        base.OnClientConnect(conn);
        MenuUIManager.Instance.ChangeToPanel(PanelType.LOBBY);
    }


    public override void OnStartHost()
    {
        Debug.LogError("@@@@OnStartHost");
        base.OnStartHost();
        MenuUIManager.Instance.ChangeToPanel(PanelType.LOBBY);
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
        isServer = false;
        networkAddress = ipAddress;
        StartClient();
    }


    public override NetworkClient StartHost()
    {
        isServer = true;
        return base.StartHost();
    }

    public void ExitLobby()
    {
        if (NetworkServer.active)
        {
            StopHost();
        }
        else
        {
            StopClient();
        }

    }
    public override void OnLobbyServerPlayersReady()
    {
        ServerChangeScene(playScene);
    }


    //本地调用 服务器不会因为多个客户端进入调用多次
    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();

    }

    public override void OnLobbyClientExit()
    {
        base.OnLobbyClientExit();
        if(mainPlayerData.lobbyPlayer != null)
        MenuUIManager.Instance.ChangeToPanel(PanelType.MENU);
    }

    public void UpdateMinPlayers()
    {
        int result = 0;
        foreach (var temp in lobbySlots)
        {
            if (temp != null) ++result;
        }
        minPlayers = result;
    }



    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        LobbyPlayer itemLobbyPlayer = conn.playerControllers[playerControllerId].gameObject.GetComponent<LobbyPlayer>();
        int index = (int)itemLobbyPlayer.shipType;
        Transform position = base.GetStartPosition();
        GameObject go = Instantiate(playerPerfabs[index], position.position, position.rotation) as GameObject;
        return go;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        if (networkSceneName == playScene)
        {
            MenuUIManager.Instance.SetVisible(false);
        }
        else if (networkSceneName == lobbyScene)
        {
            MenuUIManager.Instance.SetVisible(true);
            MenuUIManager.Instance.ChangeToPanel(PanelType.LOBBY);
        }
    }

    public override void OnLobbyServerSceneChanged(string sceneName)
    {
        //Debug.LogError("OnLobbyServerSceneChanged" + sceneName);
        if (networkSceneName == playScene)
        {
            MenuUIManager.Instance.SetVisible(false);
        }
        else if (networkSceneName == lobbyScene)
        {
            MenuUIManager.Instance.SetVisible(true);
            MenuUIManager.Instance.ChangeToPanel(PanelType.LOBBY);
        }
    }


    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //Debug.LogError("@@@@@@@OnLobbyServerSceneLoadedForPlayer" + SceneManager.GetActiveScene().name);
        LobbyPlayer itemLobbyPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        GamePlayer itemGamePlayer = gamePlayer.GetComponent<GamePlayer>();
        itemGamePlayer.InitByLobbyPlayer(itemLobbyPlayer);
        return true;
    }


    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.LogError("@@@@@@@OnClientDisconnect");
        if (networkSceneName != lobbyScene)
        {
            SceneManager.LoadScene(lobbyScene);
            MenuUIManager.Instance.SetVisible(true);
            MenuUIManager.Instance.ChangeToPanel(PanelType.MENU);
        }
        base.OnClientDisconnect(conn);
    }
    public void ReturnToStartScene()
    {
        if (NetworkServer.active)
        {
            ServerReturnToLobby();
        }
        else
        {
            SendReturnToLobby();
        }
    }
}

public enum ShipType
{
    SHIP1 = 0,
    SHIP2,
    SHIP3,
    SHIP4,
    COUNT
};
