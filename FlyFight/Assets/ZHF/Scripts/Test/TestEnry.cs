using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TestEnry : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUILayout.Button("Button1"))
        {
            ButtonFun();
        }
    }

    void ButtonFun()
    {
        Debug.LogError(NetworkServer.active);
    }
}
