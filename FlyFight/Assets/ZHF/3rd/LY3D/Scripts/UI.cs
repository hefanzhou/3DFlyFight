using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{
		public float delta = 0.0190f;
		public float zero_distance=20.0f;
		public Focus CG;
        public Material eyeMaterial;
        public Material notEyeMateial;
        public MeshRenderer screenMeshRenderer;
	    private Transform Zero_Plane;
		// Use this for initialization
		void Start ()
		{
				Screen.SetResolution (1920, 1080, true);
                Zero_Plane = CG.target;
                ToggleEyeModel(SettingPanel.Instance.GetUseEye());
                SettingPanel.Instance.OnEyeToggleEvent += ToggleEyeModel;
                Debug.Log("OnEyeToggleEvent+");
		}

        void OnDestroy()
        {
            Debug.Log("OnEyeToggleEvent-");
            SettingPanel.Instance.OnEyeToggleEvent -= ToggleEyeModel;
        }

        void OnDisable()
        {
            SettingPanel.Instance.OnEyeToggleEvent -= ToggleEyeModel;
        }

	
		void OnGUI ()
		{


				Zero_Plane.transform.localPosition=new Vector3(0,0 ,zero_distance);
				CG.delta = delta;
				CG.adjustCamera ();

                return;
                if (GUILayout.Button("Open"))
                    SettingPanel.Instance.HandleEyeToggle(true);
                if (GUILayout.Button("Close"))
                    SettingPanel.Instance.HandleEyeToggle(false);

		}
        
        public void ToggleEyeModel(bool isOpen)
        {
            Debug.Log(this.gameObject.name);
            screenMeshRenderer.material = isOpen ? eyeMaterial : notEyeMateial;
            CG.ToggleEyeModel(isOpen);
        }
        
  
}
