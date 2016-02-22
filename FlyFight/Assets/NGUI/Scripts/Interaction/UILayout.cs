using UnityEngine;
using System.Collections;

/// <summary>
/// 可对子元素重排位置
/// </summary>
public abstract class UILayout : UIWidgetContainer
{
	/// <summary>
	/// 立即重排
	/// </summary>
	public abstract void Reposition();

	/// <summary>
	/// 设置是否需要重排
	/// </summary>
	public abstract bool repositionNow
	{
		set;
	}
}
