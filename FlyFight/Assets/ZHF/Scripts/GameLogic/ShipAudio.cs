using UnityEngine;
using System.Collections;

public class ShipAudio : MonoBehaviour
{

    public AudioClip enginClip;
    public AudioClip shotClip;
    public AudioClip demageClip;
    public AudioClip deathClip;
    public AudioClip rebirthClip;

    public float engineMinVolume = 0.0f;
    public float engineMaxVolume = 1.0f;
    public float engineVolumeFadeRate = 1.0f;

    public float engineMinPitch = 1.0f;
    public float engineMaxPitch = 2.0f;
    public float enginePitchFadeRate = 1.0f;
    
    private AudioSource enginAudio;
    private AudioSource commonAudio;

    private ShipCtrl shipCtr;


    void Awake()
    {
        enginAudio = transform.Find("engin").gameObject.GetComponent<AudioSource>();
        commonAudio = transform.Find("common").gameObject.GetComponent<AudioSource>();
        shipCtr = transform.parent.gameObject.GetComponent<ShipCtrl>();
    }

	// Use this for initialization
	void Start () {
        enginAudio.clip = enginClip;
        enginAudio.loop = true;
        enginAudio.Play();
        shipCtr.OnShootEvent += PlayShootAudioHandle;
	}

    void OnEnable()
    {
        enginAudio.Play();
    }
	// Update is called once per frame
	void Update () {
        enginAudio.volume = Mathf.Lerp(enginAudio.volume, engineMinVolume + (engineMaxVolume - engineMinVolume) * shipCtr.currentSpeed / shipCtr.maxMoveSpeed, Time.deltaTime * engineVolumeFadeRate);
        enginAudio.pitch = Mathf.Lerp(enginAudio.pitch, engineMinPitch + (engineMaxPitch - engineMinPitch) * shipCtr.currentSpeed / shipCtr.maxMoveSpeed, Time.deltaTime * engineMinPitch);
	    
    }

    void PlayShootAudioHandle()
    {
        commonAudio.PlayOneShot(shotClip);
    }

    public void PlayDemageAudio()
    {
        commonAudio.PlayOneShot(demageClip);
    }

    public void PlayDeathAudio()
    {
        commonAudio.PlayOneShot(deathClip);
    }
    public void PlayRebirthAudio()
    {
        commonAudio.PlayOneShot(rebirthClip);
    }

}
