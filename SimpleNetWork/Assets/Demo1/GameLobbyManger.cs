using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
namespace Demo1
{
    public class GameLobbyManger : NetworkLobbyManager 
    {
        private static GameLobbyManger instance = null;

        public static GameLobbyManger Instance
        {
            get 
            {
                return instance;
            }
        }

        void Awake()
        {
            instance = this;
        }

        public void OnMatchCreate(CreateMatchResponse matchInfo)
        {
            
        }

        
    }
}

