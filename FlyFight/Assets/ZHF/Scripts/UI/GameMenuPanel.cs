using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuPanel : MonoBehaviour {

    private Button backBtn;


    private static GameMenuPanel instance = null;

    public static GameMenuPanel Instance
    {
        get { return GameMenuPanel.instance; }
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
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

    }

    void RegistUIEvent()
    {
        backBtn.onClick.AddListener(OnClickBack);
    }

    void OnClickBack()
    {
        GameLobbyManger.Instance.ReturnToStartScene();
    }

}
