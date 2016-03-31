using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Events;

public class MenuUIManager : MonoBehaviour
{

    public RectTransform titlePanel;
    public RectTransform menuPanle;
    public RectTransform selectPanel;
    public RectTransform lobbyPanel;

    public GameObject ServerPrefab = null;

    //menu direct link
    private Button joinBtn;
    private Button hostBtn;
    private Button quitBtn;
    //select panel


    //lobbyPanel

    //match
    private Button listButton;
    private Button createButton;

  
    public RectTransform cooldownPanel;
    private RectTransform lobbyPlayerRoot = null;
    private RectTransform ServerListRoot = null;


    [HideInInspector]
    public Text cooldownText = null;
    [HideInInspector]
    public static MenuUIManager instance = null;
    // Use this for initialization
    void Start()
    {
        InintUI();
        RegistUIEvent();
        
    }

    void InintUI()
    {
        instance = this;
        joinBtn = menuPanle.FindChild("Join").GetComponent<Button>();
        hostBtn = menuPanle.FindChild("Host").GetComponent<Button>();
        quitBtn = menuPanle.FindChild("Quit").GetComponent<Button>();

        lobbyPlayerRoot = lobbyPanel.FindChild("PlayerScrollView/Viewport/List") as RectTransform;

        //cooldownText = cooldownPanel.gameObject.GetComponentInChildren<Text>();
    }

    void RegistUIEvent()
    {
        //createButton.onClick.AddListener(StartMatch);
        //listButton.onClick.AddListener(ListServer);
        hostBtn.onClick.AddListener(OnClickHost);
        joinBtn.onClick.AddListener(OnClickJoin);
        quitBtn.onClick.AddListener(OnClickQuit);

    }

    void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();         
#endif
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void StartMatch()
    {
        GameLobbyManger.Instance.CreateMatch("comein", 4);
    }

    void OnRequsetMatchList(ListMatchResponse response)
    {
        for (int i = 0; i < response.matches.Count; ++i)
        {
            GameObject o = Instantiate(ServerPrefab) as GameObject;
            o.GetComponent<LobbyServerEntry>().Populate(response.matches[i]);
            o.transform.SetParent(ServerListRoot, false);
        }
    }
    public void ListServer()
    {
        OpenServerListPanel();
        GameLobbyManger.Instance.RequsetPage(0, OnRequsetMatchList);

    }

    public void OpenLobbyPanel()
    {
        lobbyPlayerRoot.gameObject.SetActive(true);
        ServerListRoot.gameObject.SetActive(false);
        menuPanle.gameObject.SetActive(false);
    }

    public void OpenServerListPanel()
    {
        lobbyPlayerRoot.gameObject.SetActive(false);
        ServerListRoot.gameObject.SetActive(true);
        menuPanle.gameObject.SetActive(false);
    }

    public void UpdateCooldownPanel(int countdown)
    {
        cooldownText.text = countdown + "";
        cooldownPanel.gameObject.SetActive(countdown > 0);
    }

    public void OnClickHost()
    {
        GameLobbyManger.Instance.StartHost();
    }

    public void OnClickJoin()
    {
        GameLobbyManger.Instance.DirectJoin();
    }

}
