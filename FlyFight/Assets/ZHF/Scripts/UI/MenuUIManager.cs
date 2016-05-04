using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Events;

public enum PanelType
{
    TITEL = 0,
    MENU,
    SELECT,
    LOBBY,
    COUNT
};

public class MenuUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform titlePanel;
    [SerializeField]
    private RectTransform menuPanle;
    [SerializeField]
    private RectTransform selectPanel;
    [SerializeField]
    private RectTransform lobbyPanel;
    public RectTransform cooldownPanel;

    [HideInInspector]
    public Text cooldownText = null;
    public GameObject ServerPrefab = null;

    private RectTransform currentPanel;
    private Canvas mainCanvas;


    private static MenuUIManager instance = null;

    public static MenuUIManager Instance
    {
        get { return MenuUIManager.instance; }
    }
    // Use this for initialization
    void Start()
    {
        Inint();
    }

    void Inint()
    {
        instance = this;
        currentPanel = menuPanle;
        mainCanvas = GetComponent<Canvas>();

        CloseAllPanel();
        ChangeToPanel(PanelType.MENU);
        //cooldownText = cooldownPanel.gameObject.GetComponentInChildren<Text>();
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
            //o.transform.SetParent(ServerListRoot, false);
        }
    }
    public void ListServer()
    {

        GameLobbyManger.Instance.RequsetPage(0, OnRequsetMatchList);

    }


    void CloseAllPanel()
    {
        //titlePanel.gameObject.SetActive(false);
        selectPanel.gameObject.SetActive(false);
        lobbyPanel.gameObject.SetActive(false);
        menuPanle.gameObject.SetActive(false);
    }

    public void UpdateCooldownPanel(int countdown)
    {
        cooldownText.text = countdown + "";
        cooldownPanel.gameObject.SetActive(countdown > 0);
    }

    public void ChangeToPanel(PanelType panelType)
    {
        RectTransform panel = GetPanelByType(panelType);
        currentPanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        currentPanel = panel;
    }

    private RectTransform GetPanelByType(PanelType type)
    {
        switch (type)
        {
            case PanelType.TITEL:
                return titlePanel;
                break;
            case PanelType.MENU:
                return menuPanle;
                break;
            case PanelType.SELECT:
                return selectPanel;
                break;
            case PanelType.LOBBY:
                return lobbyPanel;
                break;
            default:
                return currentPanel;
                break;
        }
    }

    public void SetVisible(bool isVisible)
    {
        transform.gameObject.SetActive(isVisible);
    }

}
