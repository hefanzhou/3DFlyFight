using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


    public class LobbyPlayer : NetworkLobbyPlayer
    {

        [SyncVar(hook="OnMyName")]
        public string playerName = "";
        [SyncVar]
        public ShipType shipType;

        public Color readyColor = Color.green;
        public Color notReadyColor = Color.red;
        private Text nameText = null;
        private Text readyText = null;
        private Image readyImage = null;
        void Awake()
        {
            nameText = transform.Find("Name").GetComponent<Text>();
            readyText = transform.Find("Ready/Text").GetComponent<Text>();
            readyImage = transform.Find("Ready").GetComponent<Image>();

        }

        void OnEnable()
        {
            SetUIReady(false);
        }
        public override void OnClientEnterLobby()
        {//生成lobbyPlayer, 当进入一个客户端  （如果房间里有已有n个客户端，那么当本地生成这n个player时，每个对象上都会调用） 适合做初始化操作

            Debug.Log("@@@@@OnClientEnterLobby" + isLocalPlayer);
            base.OnClientEnterLobby();
            LobbyPanel.Instance.AddLobbyPlayerTF(transform);
            if (!isLocalPlayer)
            {
                SetupOtherPlayer();
            }
            //其他paly第一次同步过来时 不会触发playername的hook函数， 需手动调用hook函数
            OnMyName(playerName);
        }

        public override void OnStartAuthority()
        {//获得了管理权，这里可判断为 改物体为该客户端中的主玩家
            Debug.Log("@@@OnStartAuthority");
            SetupLocalPlayer();
        }

        public override void OnClientReady(bool readyState)
        {//玩家进入准备状态
            SetUIReady(readyState);
        }

        private void SetUIReady(bool isReady)
        {
            readyText.text = isReady ? "Ready" : "NotReady";
            readyImage.color = isReady ? readyColor : notReadyColor;
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        void SetupLocalPlayer()
        {
            Debug.LogWarning("start Local" + playerName);
            GameLobbyManger.Instance.mainPlayerData.lobbyPlayer = this;
            if (playerName == "") CmdNameChanged(GameLobbyManger.Instance.mainPlayerData.name);
            shipType = GameLobbyManger.Instance.mainPlayerData.type;
        }

        void SetupOtherPlayer()
        {
            Debug.LogWarning("start other" + playerName);
        }
        public void OnMyName(string newName)
        {
            playerName = newName;
            nameText.text = playerName;
        }

        //name change, server call
        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        //name change, local call
        public void OnEditName(string str)
        {
            CmdNameChanged(str);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            Debug.Log("@@@@RpcUpdateCountdown" + countdown);
        }

        void Update()
        {
            Vector3 newPostion = transform.localPosition;
            newPostion.z = 0;
            transform.localPosition = newPostion;
        }
    }

