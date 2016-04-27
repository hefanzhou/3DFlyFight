using UnityEngine;
using System.Collections;

public class TestKinectMouse : MonoBehaviour {

	public float speed = 5;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		float Horizontal = Input.GetAxis("Horizontal");
		float Vertical = Input.GetAxis("Vertical");
		if (Horizontal != 0 || Vertical != 0)
		{
            KinectInputModule.Instance.Position += new Vector2(Horizontal,Vertical) * speed;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
            KinectInputModule.Instance.Click();
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
            //KinectMouseManager.Instance.LeftUp();
		}

	}
}
