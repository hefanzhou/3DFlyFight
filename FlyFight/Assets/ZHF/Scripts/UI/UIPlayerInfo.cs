using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour {


    private Image headImage;
    private Image hpImage;
    private Text nameText;
    private RectTransform killIconTF;

    private ShipType shipType;
	// Use this for initialization
    void Awake()
    {
        headImage = transform.Find("HeadImage").GetComponent<Image>();
        hpImage = transform.Find("Hp").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        killIconTF = transform.Find("Killed").transform as RectTransform;
    }
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(GamePlayer player)
    {
        OnHpChange(player.hp);
        OnKillNumChange(player.KillePlayerAmount);
        OnShipTypeChange(player.shipType);
        OnNameChange(player.playerName);
        player.OnKillePlayerAmountEvent += OnKillNumChange;
        player.OnHpChangeEvent += OnHpChange;
    }


    public void OnShipTypeChange(ShipType shipType)
    {
        this.shipType = shipType;
        headImage.sprite = UIPlayerInfoPanelManager.Instance.headSprites[(int)shipType];
    }
    public void OnHpChange(int hp)
    {
        if (hp < 0 || hp > 6) return;
        hpImage.sprite = UIPlayerInfoPanelManager.Instance.hpSprites[hp];
        SetDeathHeadImage(hp <= 0);
    }

    public void SetDeathHeadImage(bool isDeath)
    {
        if (isDeath) headImage.sprite = UIPlayerInfoPanelManager.Instance.deathSprite;
        else OnShipTypeChange(shipType);
    }

    public void OnKillNumChange(int num)
    {
        if (num > 3 || num < 0) return;
        for (int i = 0; i < 3; i++)
        {
            killIconTF.GetChild(i).gameObject.SetActive(i < num);
        }
       
    }

    public void OnNameChange(string name)
    {
        nameText.text = name;
    }

}
