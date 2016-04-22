using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CountDownManger : MonoBehaviour {

    public delegate void NoParmDeleagte();
    private static CountDownManger instance;


    public static CountDownManger Instance
    {
        get { return CountDownManger.instance; }
    }
    private Text countDownText;

    public CountDownManger()
    {
        instance = this;
    }

    void Awake()
    {
        countDownText = GetComponentInChildren<Text>();
    }

    public void ShowCountDown(float time, string formatStr, NoParmDeleagte callback = null, float step = 1)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(CountDown(time, formatStr, callback, step));
    }

    IEnumerator CountDown(float time, string formatStr, NoParmDeleagte callback, float step)
    {
        while (time > 0)
        {
            countDownText.text = string.Format(formatStr, time);
            yield return new WaitForSeconds(step);
            time -= step;
        }
        if(callback != null) callback();
        gameObject.SetActive(false);
    }


   
}
