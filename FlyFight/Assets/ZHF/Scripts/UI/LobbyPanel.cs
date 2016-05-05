using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BroadCast;
public class LobbyPanel : MonoBehaviour {


    private Button backBtn;
    private Button optionBtn;
    private Button joinBtn;
    private RectTransform playerList;
    private Coroutine sendBroadCastCoroutine;

    private static LobbyPanel instance = null;

    public static LobbyPanel Instance
    {
        get { return LobbyPanel.instance; }
    }
    // Use this for initialization
    void Awake()
    {
        InintUI();
    }
    
    void Start()
    {
        RegistUIEvent();

        UDPBroadCast.Instance.InitBroadCast(GameLobbyManger.Instance.mainPlayerData.name);
        sendBroadCastCoroutine = StartCoroutine(SendBroadCast(1f));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="intervalTime">发送广播间隔时间</param>
    /// <returns></returns>
    IEnumerator SendBroadCast(float intervalTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            UDPBroadCast.Instance.SendBroadCast();
        }
    }

    void InintUI()
    {
        instance = this;
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        joinBtn = transform.Find("JoinBtn").GetComponent<Button>();
        optionBtn = transform.Find("Option").GetComponent<Button>();
        playerList = transform.Find("PlayerScrollView/Viewport/List") as RectTransform;
    }

    void RegistUIEvent()
    {
        backBtn.onClick.AddListener(OnClickBackBtn);
        joinBtn.onClick.AddListener(OnClickJoinBtn);
        optionBtn.onClick.AddListener(OnClickOptionBtn);
    }

    //event
    void OnClickBackBtn()
    {
        gameObject.SetActive(false);
        MenuUIManager.Instance.ChangeToPanel(PanelType.MENU);
        GameLobbyManger.Instance.ExitLobby();
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");

        StopCoroutine(sendBroadCastCoroutine);
        UDPBroadCast.Instance.CloseBroadCast();
    }


    void OnClickJoinBtn()
    {
        GameLobbyManger.Instance.mainPlayerData.lobbyPlayer.SendReadyToBeginMessage();
    }

    void OnClickOptionBtn()
    {
 
    }

    public void AddLobbyPlayerTF(Transform lobbyTransform)
    {
        lobbyTransform.position = Vector3.zero;
        lobbyTransform.SetParent(playerList, false);
       
    }

    string TfToStr(Transform tf)
    {
        return "P:" + tf.position + "R:" + tf.rotation.eulerAngles;
    }
}
