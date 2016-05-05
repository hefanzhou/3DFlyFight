using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuPanle :MonoBehaviour, IPanelManager {

    private Button hostBtn;
    private Button joinBtn;
    private Button quitBtn;
    private Button settingBtn;
    private static MenuPanle instance = null;

    public static MenuPanle Instance
    {
        get { return MenuPanle.instance; }
    }
    // Use this for initialization
    void Start()
    {
        InintUI();
        RegistUIEvent();

    }

    void InintUI()
    {
        instance = this;
        hostBtn = transform.Find("Host").GetComponent<Button>();
        joinBtn = transform.Find("Join").GetComponent<Button>();
        quitBtn = transform.Find("Quit").GetComponent<Button>();
        settingBtn = transform.Find("Setting").GetComponent<Button>();

    }

    void RegistUIEvent()
    {
        hostBtn.onClick.AddListener(OnClickHost);
        joinBtn.onClick.AddListener(OnClickJoin);
        quitBtn.onClick.AddListener(OnClickQuit);
        settingBtn.onClick.AddListener(SettingPanel.Instance.OpenPanel);
    }

    void OnClickHost()
    {
        GameLobbyManger.Instance.StartHost();
    }

    void OnClickJoin()
    {
        ServerListPanelManager.Instance.OpenPanel();
        ClosePanle();
    }

    void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }



    public void ClosePanle()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
}
