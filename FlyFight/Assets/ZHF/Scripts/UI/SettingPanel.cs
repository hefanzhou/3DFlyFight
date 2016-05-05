using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingPanel :MonoBehaviour, IPanelManager {
    private static SettingPanel instance;

    public static SettingPanel Instance
    {
        get { return SettingPanel.instance; }
    }

    private Toggle kinectToggle;
    private Toggle eyeToggle;


    public SettingPanel()
    {
        instance = this;
    }
	void Awake () 
    {
     
        kinectToggle = transform.Find("KinectToggle").GetComponent<Toggle>();
        eyeToggle = transform.Find("EyeToggle").GetComponent<Toggle>();

        kinectToggle.isOn = GetUseKinect();
        eyeToggle.isOn = GetUseEye();

        kinectToggle.onValueChanged.AddListener(OnClickKinectToggle);
        eyeToggle.onValueChanged.AddListener(OnClickEyeToggle);
    }

	
    private void OnClickKinectToggle(bool isToggle)
    {
        KinectInputModule.Instance.UseKinectCtrl = isToggle;
        PlayerPrefs.SetInt("UseKinect", isToggle ? 1 : 0);
        
    }

    private void OnClickEyeToggle(bool isToggle)
    {
        PlayerPrefs.SetInt("UseEye", isToggle ? 1 : 0);
    }

    public bool GetUseKinect()
    {

        int result = PlayerPrefs.GetInt("UseKinect", 0);
        return result == 1 ? true : false;
    }

    public bool GetUseEye()
    {
        int result = PlayerPrefs.GetInt("UseEye", 0);
        return result == 1 ? true : false;
    }

    public void ClosePanle()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
}
