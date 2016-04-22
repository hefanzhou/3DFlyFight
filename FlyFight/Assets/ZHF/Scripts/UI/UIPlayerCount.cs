using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayerCount : MonoBehaviour {

    public Color highLightColor;
    private Image bgImage;
    private Image headImage;
    private Text nameText;
    private Text kdText;

    void Awake()
    {
        bgImage = transform.Find("BG").GetComponent<Image>();
        headImage = transform.Find("HeadImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        kdText = transform.Find("KDText").GetComponent<Text>();
    }
	
    public void SetHighLight()
    {
        bgImage.color = highLightColor;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetHeadImage(Sprite sprite)
    {
        headImage.sprite = sprite;
    }

    public void SetKdText(int kill, int died)
    {
        kdText.text = "Kill:" + kill + "  Died:" + died;
    }
}
