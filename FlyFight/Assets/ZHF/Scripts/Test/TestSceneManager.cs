using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class TestSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(SceneManager.GetActiveScene().name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
