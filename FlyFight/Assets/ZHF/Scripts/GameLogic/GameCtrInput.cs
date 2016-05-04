using UnityEngine;
using System.Collections;

public class GameCtrInput : MonoBehaviour {

    private static GameCtrInput instance;

    public static GameCtrInput Instance
    {
        get { return GameCtrInput.instance; }
    }

    public delegate void FloatParmDele(float parm);
    public delegate void BoolParmDele(bool parm);
    public delegate void NoParmDele();

    public event FloatParmDele XStickEvent;
    public event FloatParmDele YStickEvent;
    public event BoolParmDele ShootEvent;
    public event BoolParmDele BoostEvent;
    public event NoParmDele OpenMenuEvent;


    void Awake()
    {
        instance = this;
    }

	
	// Update is called once per frame
	void Update () {
        //KeyBoradInput();
	}



    public void CallXStickEvent(float Horizontal)
    {
        if(XStickEvent != null)
        XStickEvent(Horizontal);
    }

    public void CallYStickEvent(float Vertical)
    {
        if(YStickEvent != null)
        YStickEvent(Vertical);
    }

    public void CallBoostEvent(bool boost)
    {
        if(BoostEvent != null)
        BoostEvent(boost);
    }

    public void CallShootEvent(bool isShoot)
    {
        if(ShootEvent != null)
        ShootEvent(isShoot);
    }

    public void CallOpenMenuEvent()
    {
        if(OpenMenuEvent != null)
        OpenMenuEvent();
    }
}
