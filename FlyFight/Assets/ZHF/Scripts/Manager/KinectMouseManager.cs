using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum KinectCursorState
{
	nomal = 0,
	press = 1,
	draging = 2,
	count
}

public class KinectMouseManager : MonoBehaviour {

    public Image circle;


	private Texture2D[] cursorTextures = new Texture2D[(int)KinectCursorState.count];
	private Vector2 mCursorPosition = Vector2.zero;
	private static KinectMouseManager mInstance = null;
	private float xScale = 1;
	private float yScale = 1;
	public static KinectMouseManager Instance
	{
		get 
		{
			return mInstance;
		}
	}

	public Vector2 CursorPosition
	{
		get { return mCursorPosition; }
		set
		{
			mCursorPosition = value;
			MouseInput.mouse_event(MouseInput.MOUSEEVENTF_MOVE | MouseInput.MOUSEEVENTF_ABSOLUTE, (int)(value.x * xScale), (int)((Screen.height - value.y) * yScale), 0, 0);
		}
	}

	private KinectCursorState mMouseState = KinectCursorState.nomal;
	public KinectCursorState MouseState
	{
		get
		{
			return mMouseState;
		}

		set
		{
			mMouseState = value;
			ResetCursorTexture();
		}
	}



	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		mInstance = this;
		InitCursor();
	}

    void Start()
    {
        CursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);	
    }


	void ResetCursorTexture()
	{
		Texture2D cursorTexture = cursorTextures[(int)mMouseState];
		if(cursorTexture)
		{
			Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2, cursorTexture.height/2), CursorMode.Auto);
		}
	}
	void InitCursor()
	{
		CursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
		cursorTextures[(int)KinectCursorState.nomal] = Resources.Load<Texture2D>(ResPath.CursorPath[(int)KinectCursorState.nomal]);
		cursorTextures[(int)KinectCursorState.draging] = Resources.Load<Texture2D>(ResPath.CursorPath[(int)KinectCursorState.draging]);
		cursorTextures[(int)KinectCursorState.press] = Resources.Load<Texture2D>(ResPath.CursorPath[(int)KinectCursorState.press]);

		MouseState = KinectCursorState.nomal;

		xScale = 65536 / Screen.width;
		yScale = 65536 / Screen.height;		
	}



	/// <summary>
	/// 鼠标相对于当前位置的移动 ps:向上为y正方向
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void Move(int x, int y)
	{
		MouseInput.mouse_event(MouseInput.MOUSEEVENTF_MOVE, x, -y, 0, 0);
	}

	/// <summary>
	/// 左键按下并抬起，即一次点击
	/// </summary>
	public void Click()
	{
		MouseInput.mouse_event(MouseInput.MOUSEEVENTF_LEFTDOWN | MouseInput.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
	}
	
	/// <summary>
	/// 左键按下
	/// </summary>
	public void LeftPress()
	{
		MouseInput.mouse_event(MouseInput.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
		MouseState = KinectCursorState.press;
	}

	/// <summary>
	/// 左键抬起
	/// </summary>
	public void LeftUp()
	{
		MouseInput.mouse_event(MouseInput.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
		MouseState = KinectCursorState.nomal;
	}


}
