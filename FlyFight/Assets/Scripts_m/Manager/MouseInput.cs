using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;


public class MouseInput
{
	[DllImport("user32")]
	public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo); 

	public const int MOUSEEVENTF_MOVE = 0x0001;		//移动鼠标 
	public const int MOUSEEVENTF_LEFTDOWN = 0x0002;	//模拟鼠标左键按下 
	public const int MOUSEEVENTF_LEFTUP = 0x0004;		//模拟鼠标左键抬起 
	public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;	//模拟鼠标右键按下 
	public const int MOUSEEVENTF_RIGHTUP = 0x0010;		//模拟鼠标右键抬起 
	public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;	//模拟鼠标中键按下 
	public const int MOUSEEVENTF_MIDDLEUP = 0x0040;	//模拟鼠标中键抬起 
	public const int MOUSEEVENTF_ABSOLUTE = 0x8000;	//标示是否采用绝对坐标 
	
	private MouseInput() 
	{

	}
	
}
