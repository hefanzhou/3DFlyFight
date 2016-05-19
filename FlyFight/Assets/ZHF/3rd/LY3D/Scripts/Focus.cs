using UnityEngine;
using System.Collections;

public class Focus : MonoBehaviour
{
        [HideInInspector]
		public float delta = 0.25f;
		public Transform target;
        public Camera mainCamera;
        public float h = 0.036f;
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
                float singleAngle = delta / h;
                for (int i = 0; i < transform.childCount; ++i)
                {
                    float angle = (i - 3.5f) * singleAngle;
                    Transform childtransform = transform.GetChild(i);
                    childtransform.localPosition = new Vector3(h*Mathf.Sin(angle), 0,h- h*Mathf.Cos(angle));
                    childtransform.LookAt(target);
                }
		}

    /// <summary>
    /// 开启/关闭裸眼模式
    /// </summary>
        public void ToggleEyeModel(bool isOpen)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform childtransform = transform.GetChild(i);
                if(childtransform != mainCamera.transform)
                childtransform.gameObject.SetActive(isOpen);
            }
        }

}
