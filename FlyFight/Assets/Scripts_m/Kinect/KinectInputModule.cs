﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KinectInputModule : StandaloneInputModule
{
    public float affirmTime = 1f;
    private RectTransform cursorTf;
    private Image circleIm;
    private Vector2 position = Vector2.zero;
    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
            Vector2 dalta = new Vector2(Screen.width / 2, Screen.height / 2);
            screenPosition = position - dalta;
            cursorTf.localPosition = new Vector3(ScreenPosition.x, ScreenPosition.y);
        }
    }

    private Vector2 screenPosition;
    public Vector2 ScreenPosition
    {
        get
        {
            return screenPosition;
        }
    }
    private static KinectInputModule instance;

    public static KinectInputModule Instance
    {
        get { return instance; }
    }

    public delegate void HandleVG(GameObject obj);
    public event HandleVG OnKinectCursorEnterEvent;
    public event HandleVG OnKinectCursorExitEvent;

    void Awake()
    {
        instance = this;
        cursorTf = transform.FindChild("cursor").gameObject.GetComponent<RectTransform>();
        circleIm = transform.FindChild("cricle").gameObject.GetComponent<Image>();
        circleIm.fillAmount = 0;
        OnKinectCursorEnterEvent += OnKinectCursorEnter;
        OnKinectCursorExitEvent += OnKinectCursorExit;
    }

    void OnKinectCursorEnter(GameObject obj)
    {
        StopCoroutine(AffirmClick());
        StartCoroutine(AffirmClick());
    }

    void OnKinectCursorExit(GameObject obj)
    {
        StopCoroutine(AffirmClick());
    }

    private bool isClick = false;
    public void Click()
    {
        isClick = true;
    }

    IEnumerator AffirmClick()
    {
        while (true)
        {
            circleIm.fillAmount += Time.deltaTime / affirmTime;
            if (circleIm.fillAmount >= 0.99999)
            {
                circleIm.fillAmount = 0;
                Click();
                break;
            }
            yield return null;
        }
    }

    private void SetMouseButtonState(PointerEventData.FramePressState state, PointerEventData.InputButton button = PointerEventData.InputButton.Left)
    {
        MouseState m_MouseState = base.GetMousePointerEventData(0);
        PointerInputModule.ButtonState buttonState = m_MouseState.GetButtonState(button);
        buttonState.eventData.buttonState = state;
    }
    //private readonly MouseState m_MouseState = new MouseState();



    protected override MouseState GetMousePointerEventData(int id)
    {
        MouseState m_MouseState = base.GetMousePointerEventData(id);
        PointerEventData data;
        PointerEventData data2;
        PointerEventData data3;
        bool flag = this.GetPointerData(-1, out data, true);
        data.Reset();
        if (flag)
        {
            data.position = position;
        }

        Vector2 mousePosition = position;
        data.delta = mousePosition - data.position;
        data.position = mousePosition;
        data.scrollDelta = Input.mouseScrollDelta;
        data.button = PointerEventData.InputButton.Left;
        base.eventSystem.RaycastAll(data, base.m_RaycastResultCache);
        RaycastResult result = BaseInputModule.FindFirstRaycast(base.m_RaycastResultCache);
        data.pointerCurrentRaycast = result;
        base.m_RaycastResultCache.Clear();
        this.GetPointerData(-2, out data2, true);
        this.CopyFromTo(data, data2);
        data2.button = PointerEventData.InputButton.Right;
        this.GetPointerData(-3, out data3, true);
        this.CopyFromTo(data, data3);
        data3.button = PointerEventData.InputButton.Middle;
        m_MouseState.SetButtonState(PointerEventData.InputButton.Left, PointerEventData.FramePressState.NotChanged, data);
        m_MouseState.SetButtonState(PointerEventData.InputButton.Right, PointerEventData.FramePressState.NotChanged, data2);
        m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, PointerEventData.FramePressState.NotChanged, data3);
        HandleEnterAndExit(data, data.pointerCurrentRaycast.gameObject);
        ProcessClick(data);
        Debug.Log(data.pointerCurrentRaycast.gameObject);
        return m_MouseState;
    }

    void ProcessClick(PointerEventData data)
    {
        if (isClick)
        {
            SetMouseButtonState(PointerEventData.FramePressState.PressedAndReleased);
            isClick = false;
        }


    }

    void HandleEnterAndExit(PointerEventData currentPointerData, GameObject newEnterTarget)
    {
        GameObject currentEnter = currentPointerData.pointerEnter;
        if ((newEnterTarget == null) || (currentEnter == null))
        {
            for (int i = 0; i < currentPointerData.hovered.Count; i++)
            {
                Debug.Log("exit:" + currentPointerData.hovered[i].name);
                OnKinectCursorExitEvent(currentPointerData.hovered[i]);

            }
            if (newEnterTarget == null)
            {
                return;
            }
        }

        if ((currentEnter != newEnterTarget) || (newEnterTarget == null))
        {
            GameObject obj2 = FindCommonRoot(currentEnter, newEnterTarget);
            if (currentEnter != null)
            {
                for (Transform transform = currentEnter.transform; transform != null; transform = transform.parent)
                {
                    if ((obj2 != null) && (obj2.transform == transform))
                    {
                        break;
                    }
                    Debug.Log("exit:" + transform.name);
                    OnKinectCursorExitEvent(transform.gameObject);

                }
            }
            if (newEnterTarget != null)
            {
                for (Transform transform2 = newEnterTarget.transform; (transform2 != null) && (transform2.gameObject != obj2); transform2 = transform2.parent)
                {
                    Debug.Log("enter:" + transform2.name);
                    OnKinectCursorEnterEvent(transform2.gameObject);
                }
            }
        }
    }
}
