using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuPanel : MonoBehaviour, IPanelManager {

    private Button backBtn;
    private Button returnBtn;
    private Button closeMusicBtn;
    private Text closeMusicText;
    private AudioSource bgmAudioSource;

    private bool bgmIsOpen = true;
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

        closeMusicText = transform.Find("CloseMusicBtn/Text").GetComponent<Text>();
        bgmAudioSource =  GameObject.Find("/Build").GetComponent<AudioSource>();

    }

    void RegistUIEvent()
    {
        returnBtn.onClick.AddListener(OnClickBack);
        backBtn.onClick.AddListener(() => { ClosePanle(); });
        closeMusicBtn.onClick.AddListener(ToggleBgm);
        GameCtrInput.Instance.OpenMenuEvent += OpenPanel;
        
    }

    void ToggleBgm()
    {
        bgmIsOpen = !bgmIsOpen;
        if (bgmIsOpen)
        {
            closeMusicText.text = "Close Music";
            bgmAudioSource.Play();
        }
        else
        {
            closeMusicText.text = "Open Music";
            bgmAudioSource.Pause();
        }

        
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
