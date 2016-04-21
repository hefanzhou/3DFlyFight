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

    public event FloatParmDele XStickEvent;
    public event FloatParmDele YStickEvent;
    public event BoolParmDele ShootEvent;
    public event BoolParmDele BoostEvent;

    void Awake()
    {
        instance = this;
    }

	
	// Update is called once per frame
	void Update () {
        KeyBoradInput();
	}

    void KeyBoradInput()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        XStickEvent(Horizontal);
        YStickEvent(Vertical);

        BoostEvent(Input.GetKey(KeyCode.Space));
        ShootEvent(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));
    }
}
