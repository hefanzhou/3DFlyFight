using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayerCount : MonoBehaviour {

    public Color highLightColor;
    private Image bgImage;
    private Image headImage;
    private Text nameText;
    private Text kdText;

    private int killAmount = 0;
    private int deathAmount = 0;
    void Awake()
    {
        bgImage = transform.Find("BG").GetComponent<Image>();
        headImage = transform.Find("HeadImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        kdText = transform.Find("KDText").GetComponent<Text>();
    }
    public void InitByGamePlayer(GamePlayer gamePlayer)
    {
        if (gamePlayer == PVPGameManager.Instance.mineGamePlayer) SetHighLight();
        SetName(gamePlayer.playerName);
        SetHeadImage(gamePlayer.shipType);
        
        killAmount = gamePlayer.KillePlayerAmount;
        deathAmount = gamePlayer.DeathAmount;
        RefreshKdText();

        gamePlayer.OnKillePlayerAmountEvent += OnKillAmount;
        gamePlayer.OnDeathAmountEvent += OnDeathAmount;
    }
    public void SetHighLight()
    {
        bgImage.color = highLightColor;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetHeadImage(ShipType shipType)
    {
        headImage.sprite = UIPlayerInfoPanelManager.Instance.headSprites[(int)shipType];
    }

    public void RefreshKdText()
    {
        kdText.text = "Kill:" + killAmount + "  Died:" + deathAmount;
    }

    public void OnKillAmount(int kill)
    {
        killAmount = kill;
        RefreshKdText();
    }

    public void OnDeathAmount(int death)
    {
        deathAmount = death;
        RefreshKdText();
    }
}
