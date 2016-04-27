using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuPanel : MonoBehaviour, PanelManager {

    private Button backBtn;
    private Button returnBtn;
    private Button closeMusicBtn;

    private static GameMenuPanel instance = null;

    public static GameMenuPanel Instance
    {
        get { return GameMenuPanel.instance; }
    }
    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        InintUI();
        RegistUIEvent();
        gameObject.SetActive(false);
    }

    void InintUI()
    {
        instance = this;
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        returnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
        closeMusicBtn = transform.Find("CloseMusicBtn").GetComponent<Button>();

    }

    void RegistUIEvent()
    {
        returnBtn.onClick.AddListener(OnClickBack);
        backBtn.onClick.AddListener(() => { ClosePanle(); });
        GameCtrInput.Instance.OpenMenuEvent += OpenPanel;
    }


    void OnClickBack()
    {
        GameLobbyManger.Instance.ReturnToStartScene();
    }
    public void ClosePanle()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
    }

}
