using UnityEngine;
using System.Collections;

public class TestPRC : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void PrintSome()
	{
		Debug.Log("the function bing called in " + gameObject.name);
	}
}
