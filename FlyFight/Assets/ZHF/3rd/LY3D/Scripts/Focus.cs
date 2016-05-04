using UnityEngine;
using System.Collections;

public class Focus : MonoBehaviour
{
		public float delta = 0.25f;
		public Transform target;
		// Use this for initialization
		void Start ()
		{
				adjustCamera ();
		}
	
		// Update is called once per frame
		void Update ()
		{
                //adjustCamera ();
		}
		public void adjustCamera ()
		{
				Transform[] transforms = GetComponentsInChildren<Transform> ();

                for (int i = 0; i < transform.childCount; ++i)
                {
                    Transform childtransform = transform.GetChild(i);
                    childtransform.localPosition = new Vector3(-3.5f * delta + i * delta, 0, 0);
                    childtransform.LookAt(target);
                    i++;
                }

		}

}
