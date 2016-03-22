using UnityEngine;
using System.Collections;

public class ResizeRect : MonoBehaviour {

	private RectTransform mRectTf = null;
	// Use this for initialization
	void Start () {
		mRectTf = gameObject.GetComponent<RectTransform>();
		StartCoroutine(delayResize());
	}

	IEnumerator delayResize()
	{
		yield return new WaitForSeconds(0.1f);
		Resize();
	}
	public void Resize()
	{
		var rect = GetChildRect();
		mRectTf.sizeDelta = rect.size;
	}
	Rect GetChildRect()
	{
		Vector2 leftTopTemp = Vector2.zero;
		Vector2 rightButtomTemp = Vector2.zero;
		Vector2 leftTop = new Vector2(float.MaxValue, float.MinValue);
		Vector2 rightButtom = new Vector2(float.MinValue, float.MaxValue);

		for (int i = 0; i < transform.childCount; i++)
		{
			var rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
			var tf = rectTransform.rect;
			tf.position = rectTransform.anchoredPosition;

			leftTopTemp.x = tf.x - tf.width / 2;
			leftTopTemp.y = tf.y + tf.height / 2;
			rightButtomTemp.x = tf.x + tf.width / 2;
			rightButtomTemp.y = tf.y - tf.height / 2;


			leftTop.x = leftTopTemp.x < leftTop.x ? leftTopTemp.x : leftTop.x;
			leftTop.y = leftTopTemp.y > leftTop.y ? leftTopTemp.y : leftTop.y;
			rightButtom.x = rightButtomTemp.x > rightButtom.x ? rightButtomTemp.x : rightButtom.x;
			rightButtom.y = rightButtomTemp.y < rightButtom.y ? rightButtomTemp.y : rightButtom.y;
		}

		return new Rect(leftTop, new Vector2(rightButtom.x - leftTop.x, leftTop.y - rightButtom.y));
	}
}
