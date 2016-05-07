using UnityEngine;
using System.Collections;
using BroadCast;
using UnityEngine.UI;

public class ServerListPanelManager : MonoBehaviour,IPanelManager {

    public GameObject serverUIPrefab;

    private Transform listRootTF;
    private Button backBtn;
    private Text noServerText;
    private static ServerListPanelManager instance;

    public static ServerListPanelManager Instance
    {
        get { return ServerListPanelManager.instance; }
    }



    private ServerListPanelManager()
    {
        instance = this;
    }
    
    void Awake()
    {
        listRootTF = transform.Find("ServerScrollView/Viewport/List");
        noServerText = transform.Find("NoServerText").gameObject.GetComponent<Text>();
        backBtn = transform.Find("BackBtn").gameObject.GetComponent<Button>();
        backBtn.onClick.AddListener(() => { ClosePanle(); MenuPanle.Instance.OpenPanel(); });
    }


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        RefreshList();
	}

    void RefreshList()
    {
        int i = 0;
        foreach (var kvTemp in UDPBroadCast.Instance.ServerIpDic)
        {
            GameObject go;
            if (i < listRootTF.childCount)
            {
                go = listRootTF.GetChild(i).gameObject;
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(serverUIPrefab) as GameObject;
                go.transform.SetParent(listRootTF, false);
            }
            go.GetComponent<UIItemServerInfo>().Init(kvTemp.Value);
            i++;
        }

        if (i == 0) noServerText.gameObject.SetActive(true);
        else noServerText.gameObject.SetActive(false);
        for (; i < listRootTF.childCount; i++)
        {
            listRootTF.GetChild(i).gameObject.SetActive(false);
        }
    }


    void OnDestroy()
    {
        UDPBroadCast.Instance.StopListenBroadCast();
    }

    public void ClosePanle()
    {
        gameObject.SetActive(false);
        UDPBroadCast.Instance.StopListenBroadCast();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        UDPBroadCast.Instance.StartListenBroadCast();
    }

}
