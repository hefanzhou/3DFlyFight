using UnityEngine;
using System.Collections;


public class GamePlayer : MonoBehaviour {

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
