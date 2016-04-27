using UnityEngine;
using System.Collections;

public class UIPlayerInfoPanelManager : MonoBehaviour, PanelManager
{
    public Sprite[] hpSprites;
    public Sprite[] headSprites;
    public GameObject itempPlayerInfoPfb;
    public Sprite deathSprite;
    private static UIPlayerInfoPanelManager instance;

    public static UIPlayerInfoPanelManager Instance
    {
        get { return UIPlayerInfoPanelManager.instance; }
    }

    void Awake()
    {
        instance = this;
    }

    public UIPlayerInfo AddPlayerInfo()
    {
        GameObject go = Instantiate(itempPlayerInfoPfb) as GameObject;
        go.transform.SetParent(this.transform, false);
        return go.GetComponent<UIPlayerInfo>();
    }

    public void ClosePanle()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
    }

}
