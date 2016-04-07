using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


    public class LobbyPlayer : NetworkLobbyPlayer
    {
        public Text nameText = null;

        [SyncVar(hook="OnMyName")]
        public string playerName = "";

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
        }


        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        void SetupLocalPlayer()
        {
            Debug.LogWarning("start Local" + playerName);
            GameLobbyManger.Instance.mainPlayer.lobbyPlayer = this;
            if (playerName == "")
                CmdNameChanged(GameLobbyManger.Instance.mainPlayer.name);
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


    }

