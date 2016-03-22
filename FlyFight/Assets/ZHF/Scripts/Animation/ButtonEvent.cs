using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public delegate void Action();
public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler{

	public List<EventDelegate> onPointerEnter = null;
	public List<EventDelegate> onPointerExit = null;
	public List<EventDelegate> onPointerClick = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		foreach (var del in onPointerEnter)
		{
			del.Execute();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		foreach (var del in onPointerClick)
		{
			del.Execute();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		foreach (var del in onPointerExit)
		{
			del.Execute();
		}
	}
}
