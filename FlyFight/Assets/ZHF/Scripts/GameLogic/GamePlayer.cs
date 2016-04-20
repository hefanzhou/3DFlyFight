using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePlayer : NetworkManager{

	void Start () {
	    
	}

    void Awake()
    {
        PVPGameManager.Instance.AddPlayer(this);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
