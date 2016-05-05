using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BroadCast;

public class UIItemServerInfo : MonoBehaviour {

    private Text infoText;
    private Button joinBtn;
    private BroadCastMessenger mesModel;
    void Awake()
    {
        infoText = transform.Find("Info").gameObject.GetComponent<Text>();
        joinBtn = transform.Find("JoinBtn").gameObject.GetComponent<Button>();

        joinBtn.onClick.AddListener(Join);
    }

    public void Init(BroadCastMessenger mes)
    {
        mesModel = mes;
        infoText.text = "Create By:" + mes.DataBody;
    }

    void Join()
    {
        GameLobbyManger.Instance.DirectJoin(mesModel.IP);
        ServerListPanelManager.Instance.ClosePanle();
    }
}
