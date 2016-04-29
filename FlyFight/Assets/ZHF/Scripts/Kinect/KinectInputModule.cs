using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KinectInputModule : StandaloneInputModule
{
    public enum HandEventType : int
    {
        None = 0,
        Grip = 1,
        Release = 2
    }

    public float affirmTime = 1f;
    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    [Tooltip("Whether to use kinect")]
    public bool useKinectCtrl = true;

    public bool UseKinectCtrl
    {
        get { return useKinectCtrl; }
        set 
        { 
            useKinectCtrl = value;
            Cursor.visible = !useKinectCtrl;
            cursorTf.gameObject.SetActive(useKinectCtrl);
        }
    }

    [Tooltip("Smooth factor for cursor movement.")]
    public float smoothFactor = 3f;

    [Tooltip("Whether hand clicks (hand not moving for ~2 seconds) are enabled or not.")]
    public bool allowHandClicks = true;


    [Tooltip("Whether hand grips and releases control mouse dragging or not.")]
    public bool controlMouseDrag = false;

    // Bool to specify whether to convert Unity screen coordinates to full screen mouse coordinates
    //public bool convertMouseToFullScreen = false;

    private long primaryUserID = 0;

    private bool isLeftHandPrimary = false;
    private bool isRightHandPrimary = false;

    private bool isLeftHandPress = false;
    private bool isRightHandPress = false;


    private bool dragInProgress = false;

    private KinectInterop.HandState leftHandState = KinectInterop.HandState.Unknown;
    private KinectInterop.HandState rightHandState = KinectInterop.HandState.Unknown;

    private HandEventType leftHandEvent = HandEventType.None;
    private HandEventType lastLeftHandEvent = HandEventType.Release;

    private Vector3 leftHandPos = Vector3.zero;


    private Vector3 leftHandScreenPos = Vector3.zero;
    private Vector3 leftIboxLeftBotBack = Vector3.zero;
    private Vector3 leftIboxRightTopFront = Vector3.zero;
    private bool isleftIboxValid = false;
    private bool isLeftHandInteracting = false;
    private float leftHandInteractingSince = 0f;

    private Vector3 lastLeftHandPos = Vector3.zero;
    private float lastLeftHandTime = 0f;
    private bool isLeftHandClick = false;
    private float leftHandClickProgress = 0f;

    private HandEventType rightHandEvent = HandEventType.None;
    private HandEventType lastRightHandEvent = HandEventType.Release;

    private Vector3 rightHandPos = Vector3.zero;


    private Vector3 rightHandScreenPos = Vector3.zero;
    private Vector3 rightIboxLeftBotBack = Vector3.zero;
    private Vector3 rightIboxRightTopFront = Vector3.zero;
    private bool isRightIboxValid = false;
    private bool isRightHandInteracting = false;
    private float rightHandInteractingSince = 0f;

    private Vector3 lastRightHandPos = Vector3.zero;
    private float lastRightHandTime = 0f;
    private bool isRightHandClick = false;
    private float rightHandClickProgress = 0f;

    // Bool to keep track whether Kinect and Interaction library have been initialized
    private bool interactionInited = false;

    private RectTransform cursorTf;
    private Image circleIm;
    private Vector2 position = Vector2.zero;
    private Coroutine countDownClickCorotine;
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

    /// <summary>
    /// Determines whether the InteractionManager was successfully initialized.
    /// </summary>
    /// <returns><c>true</c> if InteractionManager was successfully initialized; otherwise, <c>false</c>.</returns>
    public bool IsInteractionInited()
    {
        return interactionInited;
    }

    /// <summary>
    /// Gets the current user ID, or 0 if no user is currently tracked.
    /// </summary>
    /// <returns>The user ID</returns>
    public long GetUserID()
    {
        return primaryUserID;
    }

    /// <summary>
    /// Gets the current left hand event (none, grip or release).
    /// </summary>
    /// <returns>The current left hand event.</returns>
    public HandEventType GetLeftHandEvent()
    {
        return leftHandEvent;
    }

    /// <summary>
    /// Gets the last detected left hand event (grip or release).
    /// </summary>
    /// <returns>The last left hand event.</returns>
    public HandEventType GetLastLeftHandEvent()
    {
        return lastLeftHandEvent;
    }

    /// <summary>
    /// Gets the current normalized viewport position of the left hand, in range [0, 1].
    /// </summary>
    /// <returns>The left hand viewport position.</returns>
    public Vector3 GetLeftHandScreenPos()
    {
        return leftHandScreenPos;
    }

    public Vector3 GetLeftHandPos()
    {
        return leftHandPos;
    }
    /// <summary>
    /// Determines whether the left hand is primary for the user.
    /// </summary>
    /// <returns><c>true</c> if the left hand is primary for the user; otherwise, <c>false</c>.</returns>
    public bool IsLeftHandPrimary()
    {
        return isLeftHandPrimary;
    }

    /// <summary>
    /// Determines whether the left hand is pressing.
    /// </summary>
    /// <returns><c>true</c> if the left hand is pressing; otherwise, <c>false</c>.</returns>
    public bool IsLeftHandPress()
    {
        return isLeftHandPress;
    }

    /// <summary>
    /// Determines whether a left hand click is detected, false otherwise.
    /// </summary>
    /// <returns><c>true</c> if a left hand click is detected; otherwise, <c>false</c>.</returns>
    public bool IsLeftHandClickDetected()
    {
        if (isLeftHandClick)
        {
            isLeftHandClick = false;
            leftHandClickProgress = 0f;
            lastLeftHandPos = Vector3.zero;
            lastLeftHandTime = Time.realtimeSinceStartup;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the left hand click progress, in range [0, 1].
    /// </summary>
    /// <returns>The left hand click progress.</returns>
    public float GetLeftHandClickProgress()
    {
        return leftHandClickProgress;
    }

    /// <summary>
    /// Gets the current right hand event (none, grip or release).
    /// </summary>
    /// <returns>The current right hand event.</returns>
    public HandEventType GetRightHandEvent()
    {
        return rightHandEvent;
    }

    /// <summary>
    /// Gets the last detected right hand event (grip or release).
    /// </summary>
    /// <returns>The last right hand event.</returns>
    public HandEventType GetLastRightHandEvent()
    {
        return lastRightHandEvent;
    }

    /// <summary>
    /// Gets the current normalized viewport position of the right hand, in range [0, 1].
    /// </summary>
    /// <returns>The right hand viewport position.</returns>
    public Vector3 GetRightHandScreenPos()
    {
        return rightHandScreenPos;
    }

    public Vector3 GetRightHandPos()
    {
        return rightHandPos;
    }
    /// <summary>
    /// Determines whether the right hand is primary for the user.
    /// </summary>
    /// <returns><c>true</c> if the right hand is primary for the user; otherwise, <c>false</c>.</returns>
    public bool IsRightHandPrimary()
    {
        return isRightHandPrimary;
    }

    /// <summary>
    /// Determines whether the right hand is pressing.
    /// </summary>
    /// <returns><c>true</c> if the right hand is pressing; otherwise, <c>false</c>.</returns>
    public bool IsRightHandPress()
    {
        return isRightHandPress;
    }

    /// <summary>
    /// Determines whether a right hand click is detected, false otherwise.
    /// </summary>
    /// <returns><c>true</c> if a right hand click is detected; otherwise, <c>false</c>.</returns>
    public bool IsRightHandClickDetected()
    {
        if (isRightHandClick)
        {
            isRightHandClick = false;
            rightHandClickProgress = 0f;
            lastRightHandPos = Vector3.zero;
            lastRightHandTime = Time.realtimeSinceStartup;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the right hand click progress, in range [0, 1].
    /// </summary>
    /// <returns>The right hand click progress.</returns>
    public float GetRightHandClickProgress()
    {
        return rightHandClickProgress;
    }



    public delegate void HandleVG(GameObject obj);
    public event HandleVG OnKinectCursorEnterEvent;
    public event HandleVG OnKinectCursorExitEvent;

    void Awake()
    {
        instance = this;
        cursorTf = transform.FindChild("cursor").gameObject.GetComponent<RectTransform>();
        circleIm = transform.FindChild("cursor/cricle").gameObject.GetComponent<Image>();
        circleIm.fillAmount = 0;

        OnKinectCursorEnterEvent += OnKinectCursorEnter;
        OnKinectCursorExitEvent += OnKinectCursorExit;
        //初始化鼠标位置在屏幕中间
        Position = new Vector2(Screen.width / 2, Screen.height / 2);
        interactionInited = true;
    }

    void OnDestroy()
    {
        // uninitialize Kinect interaction
        if (interactionInited)
        {
            interactionInited = false;
            instance = null;
        }
    }

    void Update()
    {
        if (!UseKinectCtrl) return;
        KinectManager kinectManager = KinectManager.Instance;

        // update Kinect interaction
        if (kinectManager && kinectManager.IsInitialized())
        {
            primaryUserID = kinectManager.GetUserIdByIndex(playerIndex);

            if (primaryUserID != 0)
            {
                HandEventType handEvent = HandEventType.None;

                // get the left hand state
                leftHandState = kinectManager.GetLeftHandState(primaryUserID);

                // check if the left hand is interacting
                isleftIboxValid = kinectManager.GetLeftHandInteractionBox(primaryUserID, ref leftIboxLeftBotBack, ref leftIboxRightTopFront, isleftIboxValid);
                //bool bLeftHandPrimaryNow = false;

                if (isleftIboxValid && //bLeftHandPrimaryNow &&
                   kinectManager.GetJointTrackingState(primaryUserID, (int)KinectInterop.JointType.HandLeft) != KinectInterop.TrackingState.NotTracked)
                {
                    leftHandPos = kinectManager.GetJointPosition(primaryUserID, (int)KinectInterop.JointType.HandLeft);

                    leftHandScreenPos.x = Mathf.Clamp01((leftHandPos.x - leftIboxLeftBotBack.x) / (leftIboxRightTopFront.x - leftIboxLeftBotBack.x));
                    leftHandScreenPos.y = Mathf.Clamp01((leftHandPos.y - leftIboxLeftBotBack.y) / (leftIboxRightTopFront.y - leftIboxLeftBotBack.y));
                    leftHandScreenPos.z = Mathf.Clamp01((leftIboxLeftBotBack.z - leftHandPos.z) / (leftIboxLeftBotBack.z - leftIboxRightTopFront.z));

                    leftHandScreenPos.x *= Screen.width;
                    leftHandScreenPos.y *= Screen.height;

                    bool wasLeftHandInteracting = isLeftHandInteracting;
                    isLeftHandInteracting = (leftHandPos.x >= (leftIboxLeftBotBack.x - 1.0f)) && (leftHandPos.x <= (leftIboxRightTopFront.x + 0.5f)) &&
                        (leftHandPos.y >= (leftIboxLeftBotBack.y - 0.1f)) && (leftHandPos.y <= (leftIboxRightTopFront.y + 0.7f)) &&
                        (leftIboxLeftBotBack.z >= leftHandPos.z) && (leftIboxRightTopFront.z * 0.8f <= leftHandPos.z);
                    //bLeftHandPrimaryNow = isLeftHandInteracting;

                    if (!wasLeftHandInteracting && isLeftHandInteracting)
                    {
                        leftHandInteractingSince = Time.realtimeSinceStartup;
                    }

                    // check for left press
                    isLeftHandPress = ((leftIboxRightTopFront.z - 0.1f) >= leftHandPos.z);

                    // check for left hand click
                    float fClickDist = (leftHandPos - lastLeftHandPos).magnitude;

                    if (allowHandClicks && !dragInProgress && isLeftHandInteracting &&
                       (fClickDist < KinectInterop.Constants.ClickMaxDistance))
                    {
                        if ((Time.realtimeSinceStartup - lastLeftHandTime) >= KinectInterop.Constants.ClickStayDuration)
                        {
                            if (!isLeftHandClick)
                            {
                                isLeftHandClick = true;
                                leftHandClickProgress = 1f;

                                    MouseControl.MouseClick();
                                    Click();
                                    isLeftHandClick = false;
                                    leftHandClickProgress = 0f;
                                    lastLeftHandPos = Vector3.zero;
                                    lastLeftHandTime = Time.realtimeSinceStartup;
                               
                            }
                        }
                        else
                        {
                            leftHandClickProgress = (Time.realtimeSinceStartup - lastLeftHandTime) / KinectInterop.Constants.ClickStayDuration;
                        }
                    }
                    else
                    {
                        isLeftHandClick = false;
                        leftHandClickProgress = 0f;
                        lastLeftHandPos = leftHandPos;
                        lastLeftHandTime = Time.realtimeSinceStartup;
                    }
                }
                else
                {
                    isLeftHandInteracting = false;
                    isLeftHandPress = false;
                }

                // get the right hand state
                rightHandState = kinectManager.GetRightHandState(primaryUserID);

                // check if the right hand is interacting
                isRightIboxValid = kinectManager.GetRightHandInteractionBox(primaryUserID, ref rightIboxLeftBotBack, ref rightIboxRightTopFront, isRightIboxValid);
                //bool bRightHandPrimaryNow = false;

                if (isRightIboxValid && //bRightHandPrimaryNow &&
                   kinectManager.GetJointTrackingState(primaryUserID, (int)KinectInterop.JointType.HandRight) != KinectInterop.TrackingState.NotTracked)
                {
                    rightHandPos = kinectManager.GetJointPosition(primaryUserID, (int)KinectInterop.JointType.HandRight);

                    rightHandScreenPos.x = Mathf.Clamp01((rightHandPos.x - rightIboxLeftBotBack.x) / (rightIboxRightTopFront.x - rightIboxLeftBotBack.x));
                    rightHandScreenPos.y = Mathf.Clamp01((rightHandPos.y - rightIboxLeftBotBack.y) / (rightIboxRightTopFront.y - rightIboxLeftBotBack.y));
                    rightHandScreenPos.z = Mathf.Clamp01((rightIboxLeftBotBack.z - rightHandPos.z) / (rightIboxLeftBotBack.z - rightIboxRightTopFront.z));

                    rightHandScreenPos.x *= Screen.width;
                    rightHandScreenPos.y *= Screen.height;

                    bool wasRightHandInteracting = isRightHandInteracting;
                    isRightHandInteracting = (rightHandPos.x >= (rightIboxLeftBotBack.x - 0.5f)) && (rightHandPos.x <= (rightIboxRightTopFront.x + 1.0f)) &&
                        (rightHandPos.y >= (rightIboxLeftBotBack.y - 0.1f)) && (rightHandPos.y <= (rightIboxRightTopFront.y + 0.7f)) &&
                        (rightIboxLeftBotBack.z >= rightHandPos.z) && (rightIboxRightTopFront.z * 0.8f <= rightHandPos.z);
                    //bRightHandPrimaryNow = isRightHandInteracting;

                    if (!wasRightHandInteracting && isRightHandInteracting)
                    {
                        rightHandInteractingSince = Time.realtimeSinceStartup;
                    }

                    if (isLeftHandInteracting && isRightHandInteracting)
                    {
                        if (rightHandInteractingSince <= leftHandInteractingSince)
                            isLeftHandInteracting = false;
                        else
                            isRightHandInteracting = false;
                    }

                    // check for right press
                    isRightHandPress = ((rightIboxRightTopFront.z - 0.1f) >= rightHandPos.z);

                    // check for right hand click
                    float fClickDist = (rightHandPos - lastRightHandPos).magnitude;

                    if (allowHandClicks && !dragInProgress && isRightHandInteracting &&
                       (fClickDist < KinectInterop.Constants.ClickMaxDistance))
                    {
                        if ((Time.realtimeSinceStartup - lastRightHandTime) >= KinectInterop.Constants.ClickStayDuration)
                        {
                            if (!isRightHandClick)
                            {
                                isRightHandClick = true;
                                rightHandClickProgress = 1f;

                                    Click();
                                    isRightHandClick = false;
                                    rightHandClickProgress = 0f;
                                    lastRightHandPos = Vector3.zero;
                                    lastRightHandTime = Time.realtimeSinceStartup;
                            }
                        }
                        else
                        {
                            rightHandClickProgress = (Time.realtimeSinceStartup - lastRightHandTime) / KinectInterop.Constants.ClickStayDuration;
                        }
                    }
                    else
                    {
                        isRightHandClick = false;
                        rightHandClickProgress = 0f;
                        lastRightHandPos = rightHandPos;
                        lastRightHandTime = Time.realtimeSinceStartup;
                    }
                }
                else
                {
                    isRightHandInteracting = false;
                    isRightHandPress = false;
                }

                // process left hand
                handEvent = HandStateToEvent(leftHandState, lastLeftHandEvent);

                if ((isLeftHandInteracting != isLeftHandPrimary) || (isRightHandInteracting != isRightHandPrimary))
                {
                    if (dragInProgress)
                    {
                        //MouseControl.MouseRelease();
                        dragInProgress = false;
                    }

                    lastLeftHandEvent = HandEventType.Release;
                    lastRightHandEvent = HandEventType.Release;
                }

                if ( (handEvent != lastLeftHandEvent))
                {
                    if (controlMouseDrag && !dragInProgress && (handEvent == HandEventType.Grip))
                    {
                        dragInProgress = true;
                        //MouseControl.MouseDrag();
                    }
                    else if (dragInProgress && (handEvent == HandEventType.Release))
                    {
                        //MouseControl.MouseRelease();
                        dragInProgress = false;
                    }
                }

                leftHandEvent = handEvent;
                if (handEvent != HandEventType.None)
                {
                    lastLeftHandEvent = handEvent;
                }

                // if the hand is primary, set the cursor position
                if (isLeftHandInteracting)
                {
                    isLeftHandPrimary = true;

                    if (UseKinectCtrl && (leftHandClickProgress < 0.8f) /**&& !isLeftHandPress*/)
                    {
                        Position = Vector3.Lerp(Position, leftHandScreenPos, smoothFactor * Time.deltaTime);
                    }
                    //					else
                    //					{
                    //						leftHandScreenPos = cursorScreenPos;
                    //					}

                    
                }
                else
                {
                    isLeftHandPrimary = false;
                }

                // process right hand
                handEvent = HandStateToEvent(rightHandState, lastRightHandEvent);

                if ((handEvent != lastRightHandEvent))
                {
                    if (controlMouseDrag && !dragInProgress && (handEvent == HandEventType.Grip))
                    {
                        dragInProgress = true;
                        //MouseControl.MouseDrag();
                    }
                    else if (dragInProgress && (handEvent == HandEventType.Release))
                    {
                        //MouseControl.MouseRelease();
                        dragInProgress = false;
                    }
                }

                rightHandEvent = handEvent;
                if (handEvent != HandEventType.None)
                {
                    lastRightHandEvent = handEvent;
                }

                // if the hand is primary, set the cursor position
                if (isRightHandInteracting)
                {
                    isRightHandPrimary = true;

                    if (UseKinectCtrl &&(rightHandClickProgress < 0.8f) /**&& !isRightHandPress*/)
                    {
                        Position = Vector3.Lerp(Position, rightHandScreenPos, smoothFactor * Time.deltaTime);
                    }
                    //					else
                    //					{
                    //						rightHandScreenPos = cursorScreenPos;
                    //					}
                   
                }
                else
                {
                    isRightHandPrimary = false;
                }

            }
            else
            {
                leftHandState = KinectInterop.HandState.NotTracked;
                rightHandState = KinectInterop.HandState.NotTracked;

                isLeftHandPrimary = false;
                isRightHandPrimary = false;

                isLeftHandPress = false;
                isRightHandPress = false;

                leftHandEvent = HandEventType.None;
                rightHandEvent = HandEventType.None;

                lastLeftHandEvent = HandEventType.Release;
                lastRightHandEvent = HandEventType.Release;

                if ( dragInProgress)
                {
                    //MouseControl.MouseRelease();
                    dragInProgress = false;
                }
            }
        }

    }


    // converts hand state to hand event type
    private HandEventType HandStateToEvent(KinectInterop.HandState handState, HandEventType lastEventType)
    {
        switch (handState)
        {
            case KinectInterop.HandState.Open:
                return HandEventType.Release;

            case KinectInterop.HandState.Closed:
            case KinectInterop.HandState.Lasso:
                return HandEventType.Grip;

            case KinectInterop.HandState.Unknown:
                return lastEventType;
        }

        return HandEventType.None;
    }

    void OnKinectCursorEnter(GameObject obj)
    {
        Debug.Log("enter:" + obj);
        circleIm.fillAmount = 0;
        if(countDownClickCorotine != null) StopCoroutine(countDownClickCorotine);
        countDownClickCorotine = StartCoroutine(AffirmClick());
    }

    void OnKinectCursorExit(GameObject obj)
    {
        Debug.Log("exit:" + obj);
        if (countDownClickCorotine != null) StopCoroutine(countDownClickCorotine);
        circleIm.fillAmount = 0;
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

    private void SetMouseButtonState(MouseState m_MouseState, PointerEventData.FramePressState state, PointerEventData.InputButton button = PointerEventData.InputButton.Left)
    {
        PointerInputModule.ButtonState buttonState = m_MouseState.GetButtonState(button);
        buttonState.eventData.buttonState = state;
    }
    //private readonly MouseState m_MouseState = new MouseState();



    protected override MouseState GetMousePointerEventData(int id)
    {   
        MouseState m_MouseState = base.GetMousePointerEventData(id);
        if (!UseKinectCtrl) return m_MouseState; 
        
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
        ProcessClick(m_MouseState);
        return m_MouseState;
    }

    void ProcessClick(MouseState m_MouseState)
    {
        if (isClick)
        {
            SetMouseButtonState(m_MouseState, PointerEventData.FramePressState.PressedAndReleased);
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
                OnKinectCursorExitEvent(currentEnter);
            }
            if (newEnterTarget != null)
            {
                OnKinectCursorEnterEvent(newEnterTarget);
            }
        }
    }
}
