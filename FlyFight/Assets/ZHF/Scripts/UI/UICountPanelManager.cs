using UnityEngine;
using System.Collections;

public class UICountPanelManager : MonoBehaviour {
    
    public GameObject itempPlayerConutPfb;
    private Transform listRoot;
    private static UICountPanelManager instance;

    public static UICountPanelManager Instance
    {
        get { return UICountPanelManager.instance; }
    }

    void Awake()
    {
        instance = this;
        listRoot = transform.Find("CountList");
    }

    public UIPlayerInfo AddPlayerInfo()
    {
        GameObject go = Instantiate(itempPlayerConutPfb) as GameObject;
        go.transform.SetParent(listRoot, false);
        return go.GetComponent<UIPlayerInfo>();
    }

}
