using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuPanle : MonoBehaviour {

    private Button hostBtn;
    private Button joinBtn;
    private Button quitBtn;

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
    }

    void RegistUIEvent()
    {
        hostBtn.onClick.AddListener(OnClickHost);
        joinBtn.onClick.AddListener(OnClickJoin);
        quitBtn.onClick.AddListener(OnClickQuit);
    }

    void OnClickHost()
    {
        GameLobbyManger.Instance.StartHost();
    }

    void OnClickJoin()
    {
        GameLobbyManger.Instance.DirectJoin();
    }

    void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
