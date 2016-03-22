using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ImageAnimation : MonoBehaviour {

	[SerializeField]
	protected float speed = 0.1f;
	[HideInInspector]
	[SerializeField]
	protected string mPrefix = "";
	[SerializeField]
	protected bool mLoop = true;
	[HideInInspector]
	[SerializeField]
	protected bool mSnap = true;
	[SerializeField]
	protected int mCount = 1;
	protected Image mImage;
	protected float mDelta = 0f;
	protected int mIndex = 0;
	protected bool mActive = true;
	protected List<Sprite> mSpriteList = new List<Sprite>();

	/// <summary>
	/// Number of frames in the animation.
	/// </summary>





	/// <summary>
	/// Set the name prefix used to filter sprites from the atlas.
	/// </summary>

	public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>

	public bool loop { get { return mLoop; } set { mLoop = value; } }

	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>

	public bool isPlaying { get { return mActive; } }

	/// <summary>
	/// Rebuild the sprite list first thing.
	/// </summary>

	protected virtual void Start() { RebuildSpriteList(); }

	/// <summary>
	/// Advance the sprite animation process.
	/// </summary>

	protected virtual void Update()
	{
		if (mActive && mSpriteList.Count > 1 && Application.isPlaying && speed > 0)
		{
			mDelta += RealTime.deltaTime;
			float rate = speed;

			if (rate < mDelta)
			{

				mDelta = (rate > 0f) ? mDelta - rate : 0f;

				if (++mIndex >= mCount)
				{
					mIndex = 0;
					mActive = mLoop;
				}

				if (mActive)
				{
					mImage.overrideSprite = mSpriteList[mIndex];
				}
			}
		}
	}

	/// <summary>
	/// Rebuild the sprite list after changing the sprite name.
	/// </summary>

	public void RebuildSpriteList()
	{
		if (mImage == null) mImage = GetComponent<Image>();
		mSpriteList.Clear();

		if (mImage != null )
		{

			for (int i = 1, imax = mCount; i <= imax; ++i)
			{
				Sprite temp = Resources.Load<Sprite>(string.Format(ResPath.BgAnimationImage, i));
				if (temp) mSpriteList.Add(temp);
			}
		}
	}

	/// <summary>
	/// Reset the animation to the beginning.
	/// </summary>

	public void Play() { mActive = true; }

	/// <summary>
	/// Pause the animation.
	/// </summary>

	public void Pause() { mActive = false; }


}
