using UnityEngine;
using System.Collections;

public class AddCameraToUI : MonoBehaviour {

    public Camera targetCamera;
	void Start () {
        StartCoroutine(SetWordCamera());
	}

    IEnumerator SetWordCamera()
    {
        while (true)
        {
            GameObject go = GameObject.Find("/StartMenuRoot/StartMenu");
            if (go != null)
            {
                go.GetComponent<Canvas>().worldCamera = targetCamera;
                break;
            }
            yield return null;
        }
    }
}
