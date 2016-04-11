using UnityEngine;
using System.Collections;

public class CreateManager : MonoBehaviour {
    public GameObject managerGo;
    void Awake()
    {
        if (GameLobbyManger.Instance == null)
        {
            Instantiate(managerGo);
        }
    }
}
