//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 自动复制 template 元素，再按 UITable 的方式定位元素
/// </summary>

[AddComponentMenu("NGUI/Interaction/List")]
public class UIList : UILayout
{
	public delegate void OnReposition();

	public enum Direction
	{
		Down,
		Up,
	}

	/// <summary>
	/// How many columns there will be before a new line is started. 0 means unlimited.
	/// </summary>

	public int columns = 0;

	/// <summary>
	/// Which way the new lines will be added.
	/// </summary>

	public Direction direction = Direction.Down;

	/// <summary>
	/// 用于复制的元素模板
	/// </summary>

	public GameObject template;

	/// <summary>
	/// 元素数量
	/// </summary>

	public int itemCount = 1;

	/// <summary>
	/// 复制时是否重命名所有元素。重命名方式 originalName_%d
	/// </summary>

	public bool renameControl = false;

	/// <summary>
	/// Whether the parent container will be notified of the table's changes.
	/// </summary>

	public bool keepWithinPanel = false;

	/// <summary>
	/// Padding around each entry, in pixels.
	/// </summary>

	public Vector2 padding = Vector2.zero;

	/// <summary>
	/// Delegate function that will be called when the table repositions its content.
	/// </summary>

	public OnReposition onReposition;

	////cache item list, so that item can be access by index
	//private List<GameObject> mItemList = new List<GameObject>();
	//public IList<GameObject> Items { get { return mItemList; } }

	protected UIPanel mPanel;
	protected bool mInitDone = false;
	protected bool mReposition = false;
	protected List<GameObject> mChildren = new List<GameObject>();
	/// <summary>
	/// 原有第一次调用 Resize 前的元素数量
	/// </summary>
	protected int mResizeHiddenItems = -1;

	/// <summary>
	/// Reposition the children on the next Update().
	/// </summary>

	public override bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }

	/// <summary>
	/// Returns the list of table's children, sorted alphabetically if necessary.
	/// </summary>

	public List<GameObject> children
	{
		get
		{
			return mChildren;
		}
	}

	/// <summary>
	/// Positions the grid items, taking their own size into consideration.
	/// </summary>

	protected void RepositionVariableSize(List<GameObject> children)
	{
		float xOffset = 0;
		float yOffset = 0;

		int cols = columns > 0 ? children.Count / columns + 1 : 1;
		int rows = columns > 0 ? columns : children.Count;

		Bounds[,] bounds = new Bounds[cols, rows];
		Bounds[] boundsRows = new Bounds[rows];
		Bounds[] boundsCols = new Bounds[cols];

		int x = 0;
		int y = 0;

		for (int i = 0, imax = children.Count; i < imax; ++i)
		{
			Transform t = children[i].transform;
			Bounds b = NGUIMath.CalculateRelativeWidgetBounds(t, NGUIMath.ConsiderActiveType.activeSelf);
            
			Vector3 scale = t.localScale;
			b.min = Vector3.Scale(b.min, scale);
			b.max = Vector3.Scale(b.max, scale);
			bounds[y, x] = b;

			boundsRows[x].Encapsulate(b);
			boundsCols[y].Encapsulate(b);

			if (++x >= columns && columns > 0)
			{
				x = 0;
				++y;
			}
		}

		x = 0;
		y = 0;

		for (int i = 0, imax = children.Count; i < imax; ++i)
		{
			Transform t = children[i].transform;
			Bounds b = bounds[y, x];
			Bounds br = boundsRows[x];
			Bounds bc = boundsCols[y];

			Vector3 pos = t.localPosition;
			pos.x = xOffset + b.extents.x - b.center.x;
			pos.x += b.min.x - br.min.x + padding.x;

			if (direction == Direction.Down)
			{
				pos.y = -yOffset - b.extents.y - b.center.y;
				pos.y += (b.max.y - b.min.y - bc.max.y + bc.min.y) * 0.5f - padding.y;
			}
			else
			{
				pos.y = yOffset + (b.extents.y - b.center.y);
				pos.y -= (b.max.y - b.min.y - bc.max.y + bc.min.y) * 0.5f - padding.y;
			}

			xOffset += br.max.x - br.min.x + padding.x * 2f;

			t.localPosition = pos;

			if (++x >= columns && columns > 0)
			{
				x = 0;
				++y;

				xOffset = 0f;
				yOffset += bc.size.y + padding.y * 2f;
			}
		}
	}

	public virtual void Resize()
	{
		if (Application.isPlaying && !mInitDone && NGUITools.GetActive(this))
		{
			ResizeInternal();
		}
		else
		{
			Reposition();
        }
	}

	private void RenameControlRecursiveList(Transform node, int subfix)
	{
		node.gameObject.name += "_" + subfix;
		foreach (Transform childTrans in node)
		{
			RenameControlRecursiveList(childTrans, subfix);
		}
	}

	protected virtual void ResizeInternal()
	{
		Transform myTrans = transform;

		mChildren.Clear();

		if (mResizeHiddenItems < 0)	//第一次 resize 前
		{
			foreach (Transform childTrans in myTrans)
			{
				if (childTrans.gameObject != template)
					childTrans.gameObject.name = "_hidden";
				childTrans.gameObject.SetActive(false);
			}
			mResizeHiddenItems = myTrans.childCount;
		}

		//adjust item count
		if (template != null)
		{
			template.SetActive(false);

			int extraItemCount = mResizeHiddenItems;

			int currentItemCount = myTrans.childCount - extraItemCount;
			if (currentItemCount < itemCount)	//need add
			{
				for (int i = currentItemCount; i < itemCount; ++i)
				{
					GameObject newObj = (GameObject)Instantiate(template, template.transform.position, template.transform.rotation);
					if (renameControl)
						newObj.name = template.name + "_" + (i + 1);
					else
						newObj.name = "item_" + (i + 1);

					newObj.transform.parent = template.transform.parent;
					newObj.transform.localPosition = template.transform.localPosition;
					newObj.transform.localScale = template.transform.localScale;

					if (renameControl)
					{
						foreach (Transform childTrans in newObj.transform)
						{
							RenameControlRecursiveList(childTrans, i + 1);
						}
					}
				}
			}

			//显示的
			for (int i=0; i<itemCount; ++i)
			{
				Transform t = myTrans.GetChild(i + extraItemCount);
				t.gameObject.SetActive(true);
				mChildren.Add(t.gameObject);
			}

			//删除的，从后向前删除
			int childCount = myTrans.childCount;
			for (int i = 0; i < childCount - extraItemCount - itemCount; ++i)
			{
				int index = childCount - i - 1;
				Transform t = myTrans.GetChild(index);
				t.gameObject.SetActive(false);
				t.parent = null;
				Object.Destroy(t.gameObject);
			}
		}
	}

	/// <summary>
	/// Recalculate the position of all elements within the table, create new elements if need
	/// </summary>

	[ContextMenu("Execute")]
	public override void Reposition()
	{
		if (Application.isPlaying && !mInitDone && NGUITools.GetActive(this))
		{
			mReposition = true;
			return;
		}

		if (!mInitDone) Init();

		ResizeInternal();

		Transform myTrans = transform;

		mReposition = false;

		List<GameObject> ch = children;
        if (ch.Count > 0)
        {
            gameObject.SetActive(true);
            RepositionVariableSize(ch);
        }

		if (keepWithinPanel && mPanel != null)
		{
			mPanel.ConstrainTargetToBounds(myTrans, true);
			UIScrollView sv = mPanel.GetComponent<UIScrollView>();
			if (sv != null) sv.UpdateScrollbars(true);

		}

		if (onReposition != null)
			onReposition();
	}

	/// <summary>
	/// Position the grid's contents when the script starts.
	/// </summary>

	protected virtual void Start()
	{
		Init();
		Reposition();
		enabled = false;
	}

	/// <summary>
	/// Find the necessary components.
	/// </summary>

	protected virtual void Init()
	{
		mInitDone = true;
		mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
	}

	/// <summary>
	/// Is it time to reposition? Do so now.
	/// </summary>

	protected virtual void LateUpdate()
	{
		if (mReposition) Reposition();
		enabled = false;
	}

	///// <summary>
	///// Drag the draggable panel (if presents) to offset at given index
	///// </summary>
	///// <param name="itemIndex"></param>
	///// <param name="strength"></param>
	//public void DragTo(int itemIndex, float strength)
	//{
	//    if (mPanel == null)
	//        return;

	//    UIScrollView sv = mPanel.GetComponent<UIScrollView>();
	//    if (sv == null)
	//        return;

	//    if (itemIndex < 0 || itemIndex >= children.Count)
	//        return;
		
	//    GameObject target = children[itemIndex];

	//    Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(sv.transform, target.transform);
	//    Bounds scrollViewBounds = sv.bounds;

	//    Vector3 dragPos = sv.CalcPosByDragDistance(targetBounds.min.x - scrollViewBounds.min.x, scrollViewBounds.max.y - targetBounds.max.y);

	//    SpringPanel.Begin(mPanel.cachedGameObject, dragPos, strength);
	//}

	/// <summary>
	/// Drag the draggable panel (if presents), to make sure item with given index is visible
	/// </summary>
	/// <param name="itemIndex"></param>
	/// <param name="strength"></param>
	public void DragToMakeVisible(int itemIndex, float strength)
	{
		if (mPanel == null)
			return;

		UIScrollView sv = mPanel.GetComponent<UIScrollView>();
		if (sv == null)
			return;

		if (itemIndex < 0 || itemIndex >= children.Count)
			return;

		GameObject target = children[itemIndex];
		sv.DragToMakeVisible(target.transform, strength);
	}
}
