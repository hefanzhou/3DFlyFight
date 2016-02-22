using UnityEngine;
using System.Collections;

namespace Demo1
{
    public class UIManager : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void StartMatch()
        {
            GameLobbyManger.Instance.StartMatchMaker();
            GameLobbyManger.Instance.matchMaker.CreateMatch(
                "demo1",
                4,
                true,
                "",
                GameLobbyManger.Instance.OnMatchCreate);
        }
    }
}

