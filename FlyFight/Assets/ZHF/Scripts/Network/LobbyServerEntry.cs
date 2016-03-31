using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

    public class LobbyServerEntry : MonoBehaviour
    {
        public Text serverInfoText;
        public Text slotInfo;
        public Button joinButton;


        private MatchDesc curMatch;


        public void Populate(MatchDesc match)
        {
            curMatch = match;
            serverInfoText.text = match.name;

            slotInfo.text = match.currentSize.ToString() + "/" + match.maxSize.ToString(); ;

            NetworkID networkID = match.networkId;

            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(() => { GameLobbyManger.Instance.JoinMatch(networkID); });

        }


    }
