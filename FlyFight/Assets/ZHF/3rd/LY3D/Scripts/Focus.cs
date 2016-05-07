using UnityEngine;
using System.Collections;

public class Focus : MonoBehaviour
{
        [HideInInspector]
		public float delta = 0.25f;
		public Transform target;
        public Camera mainCamera;
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
