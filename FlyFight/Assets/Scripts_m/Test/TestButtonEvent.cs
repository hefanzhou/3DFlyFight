using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestButtonEvent : MonoBehaviour {
    public GameObject button;
    public EventSystem m_eventSystem;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if(GUILayout.Button("Enter"))
        {
            ExecuteEvents.Execute<IPointerEnterHandler>(button, new PointerEventData(m_eventSystem), ExecuteEvents.pointerEnterHandler);
        }
    }
}
