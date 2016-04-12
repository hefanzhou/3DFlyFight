using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class TestSceneManager : MonoBehaviour {

	// Use this for initialization
    Scene[] sceneArray;
	void Start () {
        DontDestroyOnLoad(gameObject);
        sceneArray = SceneManager.GetAllScenes();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        sceneArray = SceneManager.GetAllScenes();
        foreach (var scene in sceneArray)
        {
            if (GUILayout.Button(scene.name))
            {
                SceneManager.LoadScene(scene.name);
            }
        }

        if (GUILayout.Button("GetActiveScene"))
        {
            Debug.LogError(SceneManager.GetActiveScene().name);
            Debug.LogError(SceneManager.sceneCount);
        }

        if (GUILayout.Button("Jump"))
        {
            SceneManager.LoadScene(2);
        }
    }

   
}
