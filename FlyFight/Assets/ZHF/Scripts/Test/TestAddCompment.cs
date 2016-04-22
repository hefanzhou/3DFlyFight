using UnityEngine;
using System.Collections;

public class TestAddCompment : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUILayout.Button("Call"))
        {
            Debug.Log("Call");
            CountDownManger.Instance.ShowCountDown(3, "{0: 0.00}scends relife...", () => { Debug.Log("Complet!!"); }, 0.01f);
        }
    }
}
