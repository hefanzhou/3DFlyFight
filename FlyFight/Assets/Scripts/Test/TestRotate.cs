using UnityEngine;
using System.Collections;

public class TestRotate : MonoBehaviour {

	public Transform origTransfom = null;
	public float angle = 5;
	private Quaternion targetQua;
	private Vector3 beginAngel;
	// Use this for initialization
	void Start () {
		targetQua = Quaternion.AngleAxis(angle, Vector3.up);
		beginAngel = transform.position - origTransfom.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A))
		{
			//StartCoroutine(BeginRotate());
			transform.RotateAround(origTransfom.position, Vector3.up, angle * Time.deltaTime);
		}
	}
	
	IEnumerator BeginRotate()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f);
		}
	}
}
