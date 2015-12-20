using UnityEngine;
using System.Collections;

public class RenderTransparencyOnBottom : MonoBehaviour {
	
	void Start () {
		this.GetComponent<Renderer>().material.renderQueue = 3000;
	}
	
}
