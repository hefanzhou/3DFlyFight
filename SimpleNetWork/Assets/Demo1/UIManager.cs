using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Demo
{
    public class UIManager : MonoBehaviour
    {
        public Button createButton;
        public Button listButton;
        public Button joinBtn;
        public Button hostBtn;

        public RectTransform menuPanle;
        public RectTransform cooldownPanel;
        public RectTransform lobbyPlayerRoot = null;
        public RectTransform ServerListRoot = null;

        public GameObject ServerPrefab = null;

        [HideInInspector]
        public Text cooldownText = null;
        [HideInInspector]
        public static UIManager instance = null;
        // Use this for initialization
        void Start()
        {
            instance = this;
            createButton.onClick.AddListener(StartMatch);
            listButton.onClick.AddListener(ListServer);
            hostBtn.onClick.AddListener(OnClickHost);
            joinBtn.onClick.AddListener(OnClickJoin);
            cooldownText = cooldownPanel.gameObject.GetComponentInChildren<Text>();
        }

        // Update is called once per frame
        void Update()
        {

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
                o.transform.SetParent(ServerListRoot, false);
            }
        }
        public void ListServer()
        {
            OpenServerListPanel();
            GameLobbyManger.Instance.RequsetPage(0, OnRequsetMatchList);

        }

        public void OpenLobbyPanel()
        {
            lobbyPlayerRoot.gameObject.SetActive(true);
            ServerListRoot.gameObject.SetActive(false);
            menuPanle.gameObject.SetActive(false);
        }

        public void OpenServerListPanel()
        {
            lobbyPlayerRoot.gameObject.SetActive(false);
            ServerListRoot.gameObject.SetActive(true);
            menuPanle.gameObject.SetActive(false);
        }

        public void UpdateCooldownPanel(int countdown)
        {
            cooldownText.text = countdown + "";
            cooldownPanel.gameObject.SetActive(countdown > 0);
        }

        public void OnClickHost()
        {
            GameLobbyManger.Instance.StartHost();
        }

        public void OnClickJoin()
        {
            GameLobbyManger.Instance.DirectJoin();
        }

    }
}

