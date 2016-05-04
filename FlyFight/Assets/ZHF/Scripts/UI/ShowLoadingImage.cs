using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShowLoadingImage : MonoBehaviour {

    public float hideTime = 2;
    private Image loadImage;
	// Use this for initialization
	void Start () {
        loadImage = GetComponent<Image>();
        StartCoroutine(DelayHide());
	}

    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(hideTime);
        gameObject.SetActive(false);
    }


}
