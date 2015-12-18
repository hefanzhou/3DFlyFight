using UnityEngine;
using System.Collections;

public class LogManager : MonoBehaviour
{
	private UILabel		lable = null;
	private UITextList	textList = null;
	private UIButton	clearBtn = null;
	private UIButton	toggleBtn = null;
	private GameObject	panel = null;

	private string[] logStr = { "[FF4040]{0}[-]", "[FF4040]{0}[-]", "[EEC900]{0}[-]", "[A2CD5A]{0}[-]", "[FF4040]{0}[-]" };

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		DontDestroyOnLoad(transform.parent.gameObject);
	}
	void Start () {
		InitProperty();
	}
	

	void InitProperty()
	{
		lable		= transform.Find("panel/Label").GetComponent<UILabel>();
		textList	= transform.Find("panel/TextList").GetComponent<UITextList>();
		clearBtn	= transform.Find("panel/ClearBtn").GetComponent<UIButton>();
		toggleBtn	= transform.Find("ToggleBtn").GetComponent<UIButton>();
		panel		= transform.Find("panel").gameObject; 

		UIEventListener.Get(clearBtn.gameObject).onClick += OnClearBtnClick;
		UIEventListener.Get(toggleBtn.gameObject).onClick += OnToggleBtnClick;
		Application.RegisterLogCallback(HandleLog);
	
	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (type.Equals(LogType.Error) || type.Equals(LogType.Exception) || type.Equals(LogType.Assert)) logString += "/n" + stackTrace;
		textList.Add(string.Format(logStr[(int)type], logString));
	}

	void OnClearBtnClick(GameObject _)
	{
		textList.Clear();
	}

	void OnToggleBtnClick(GameObject _)
	{
		panel.SetActive(!panel.active);
	}
}
