﻿using UnityEngine;
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
    private Button backBtn;

    [HideInInspector]
    public event System.Action<bool> OnEyeToggleEvent;


	void Awake () 
    {
        instance = this;
        kinectToggle = transform.Find("KinectToggle").GetComponent<Toggle>();
        eyeToggle = transform.Find("EyeToggle").GetComponent<Toggle>();
        backBtn = transform.Find("BackBtn").gameObject.GetComponent<Button>();

        kinectToggle.isOn = GetUseKinect();
        eyeToggle.isOn = GetUseEye();

        kinectToggle.onValueChanged.AddListener(OnClickKinectToggle);
        eyeToggle.onValueChanged.AddListener(HandleEyeToggle);
        OnEyeToggleEvent += OnClickEyeToggle;
        backBtn.onClick.AddListener(BackToMenu);
        gameObject.SetActive(false);
    }


    void BackToMenu()
    {
        ClosePanle(); 
        MenuPanle.Instance.OpenPanel();
    }

    public void HandleEyeToggle(bool isOpen)
    {
        OnEyeToggleEvent(isOpen);
    }


	
    private void OnClickKinectToggle(bool isToggle)
    {
        KinectInputModule.Instance.UseKinectCtrl = isToggle;
        PlayerPrefs.SetInt("UseKinect", isToggle ? 1 : 0);
        
    }

    private void OnClickEyeToggle(bool isToggle)
    {
        Debug.Log("OnClickEyeToggle");
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
