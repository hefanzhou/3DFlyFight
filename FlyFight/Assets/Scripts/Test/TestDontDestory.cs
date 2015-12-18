using UnityEngine;
using System.Collections;

public class TestDontDestory : MonoBehaviour {

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(transform.parent.gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
