using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UGUIPlaySound : MonoBehaviour {


	public AudioClip audioClip;

	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0f, 2f)]
	public float pitch = 1f;


	public void Play()
	{
		NGUITools.PlaySound(audioClip, volume, pitch);
	}
}
