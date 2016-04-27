using UnityEngine;
using System.Collections;

public class PvpGameUIManger : MonoBehaviour {


    private static PvpGameUIManger instance;

    public static PvpGameUIManger Instance
    {
        get { return PvpGameUIManger.instance; }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

interface PanelManager
{
    void ClosePanle();
    void OpenPanel();
}