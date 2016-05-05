using UnityEngine;
using System.Collections;

public class AddCameraToUI : MonoBehaviour {

    public Camera targetCamera;
    
	void Start () {
        GameObject.Find("/StartMenu").GetComponent<Canvas>().worldCamera = targetCamera;
	}
	
}
