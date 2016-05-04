using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{
		float delta = 0.0190f;
		float zero_distance=20.0f;
		public Focus CG;
	public Transform Zero_Plane;
		// Use this for initialization
		void Start ()
		{
				Screen.SetResolution (1920, 1080, true);
		}
	

	
		void OnGUI ()
		{
				/*
				if (GUI.Button (new Rect (Screen.width - 100, Screen.height - 50, 100, 50), "关闭")) {
						Application.Quit ();
				}
				*/
				//delta = GUI.HorizontalSlider (new Rect (20, 20, 1880, 20), delta, 0, 0.8f);
				//GUI.Label(new Rect (0, 20, 40, 20),delta.ToString());
				//zero_distance=GUI.HorizontalSlider (new Rect (20, 40, 1880, 20), zero_distance, 0, 50);
				//GUI.Label(new Rect (0, 40, 40, 20),zero_distance.ToString());
				Zero_Plane.transform.localPosition=new Vector3(0,0 ,zero_distance);
				CG.delta = delta;
				CG.adjustCamera ();
		}
}
