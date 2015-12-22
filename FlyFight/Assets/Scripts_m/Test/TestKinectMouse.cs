using UnityEngine;
using System.Collections;

public class TestKinectMouse : MonoBehaviour {

	public int speed = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float Horizontal = Input.GetAxis("Horizontal");
		float Vertical = Input.GetAxis("Vertical");

		if (Horizontal != 0 || Vertical != 0)
		{
			KinectMouseManager.Instance.Move((int)(Horizontal * speed), (int)(Vertical * speed));
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			KinectMouseManager.Instance.LeftPress();
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			KinectMouseManager.Instance.LeftUp();
		}

		if (Input.GetKey(KeyCode.Q))
		{
			Debug.Log(Screen.width / 2 + "/" + Screen.height / 2);
			KinectMouseManager.Instance.CursorPosition = new Vector2(Screen.width / 2, Screen.height / 2); ;
		}
	}
}
