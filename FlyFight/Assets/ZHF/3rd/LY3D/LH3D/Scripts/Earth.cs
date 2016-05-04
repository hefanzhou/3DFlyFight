using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.forward ,10.0f*Time.deltaTime )   ; 
	}
}
