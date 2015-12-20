using UnityEngine;
using System.Collections;

public class RenderOnTop : MonoBehaviour {
	
	void Start () {
		this.GetComponent<Renderer>().material.renderQueue = 4000;
	}
	
}
