using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UICountPanelManager : MonoBehaviour, PanelManager {
    
    public GameObject itempPlayerConutPfb;
    private Transform listRoot;
    private static UICountPanelManager instance;
    private Button returnBtn;
    public static UICountPanelManager Instance
    {
        get { return UICountPanelManager.instance; }
    }

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
        listRoot = transform.Find("CountList");
        RegisterBtnEvent();
    }

    void RegisterBtnEvent()
    {
        returnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
        returnBtn.onClick.AddListener(() => { GameLobbyManger.Instance.ReturnToStartScene(); });
    }

    void Init()
    {
        var playerList = PVPGameManager.Instance.gamePlayerList;
        playerList.Sort();
        foreach (var itemPlayer in playerList)
        {
            AddPlayerInfo(itemPlayer);
        }
    }

    private void AddPlayerInfo(GamePlayer gamePlayer)
    {
        GameObject go = Instantiate(itempPlayerConutPfb) as GameObject;
        go.transform.SetParent(listRoot, false);
        UIPlayerCount uiPlayerCount = go.GetComponent<UIPlayerCount>();
        uiPlayerCount.InitByGamePlayer(gamePlayer);
    }


    public void ClosePanle()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
        Init();
    }
}
