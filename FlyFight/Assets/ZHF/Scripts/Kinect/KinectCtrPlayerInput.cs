using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class KinectCtrPlayerInput : MonoBehaviour {
    
    [Range(0.1f, 0.8f)]
    public float minRecognitionSin = 0.17f;
    [Range(0.1f, 0.8f)]
    public float maxRecognitionSin = 0.48f;

    [Range(0.1f, 0.8f)]
    public float minRecognitionHandSin = 0.1f;
    [Range(0.1f, 0.8f)]
    public float maxRecognitionHandSin = 0.8f;
    
    [Range(2f, 4f)]
    public float minHandDisScale = 2;

    private const float OFFSET_Z = 0.13f;
	// Use this for initialization
    private  Action ctrFunction;
	void Start () {

        if (KinectInputModule.Instance.UseKinectCtrl) ctrFunction = KinectInput;
        else ctrFunction = KeyBoradInput;
       
        outputText = transform.Find("OutPutText").GetComponent<Text>();
	}

    private int playerIndex = 0;
    private string outputStr;
    private Text outputText;
    private long primaryUserID;
    private KinectInterop.HandState leftHandState;
    private KinectInterop.HandState rightHandState;
    private Vector3 leftHandPos = Vector3.zero;
    private Vector3 rightHandPos = Vector3.zero;
    private Vector3 spineShoulderPos = Vector3.zero;
    private Vector3 spineBasePos = Vector3.zero;
    private Vector3 originPos = Vector3.zero;

    private float shoulderBaseDis = 0.1f;

	// Update is called once per frame
	void Update () {

        ctrFunction();
	}

    void KinectInput()
    {
        
        if (RefreshJointPos())
        {
            AnalysePlayerCtrByShouderBase();
            AnalysePlayerCtrByHand();
        }
    }

    void KeyBoradInput()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        GameCtrInput.Instance.CallXStickEvent(Horizontal);
        GameCtrInput.Instance.CallYStickEvent(Vertical);

        GameCtrInput.Instance.CallBoostEvent(Input.GetKey(KeyCode.Space));
        GameCtrInput.Instance.CallShootEvent(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameCtrInput.Instance.CallOpenMenuEvent();
        }
    }

    void FixedUpdate()
    {
        shoulderBaseDis = Vector3.Distance(spineShoulderPos, spineBasePos);
        
        //双手识别
        minHandDis = shoulderBaseDis * minHandDisScale;


        //腰识别
        minThresholdValue = shoulderBaseDis * minRecognitionSin;
        maxThresholdValue = shoulderBaseDis * maxRecognitionSin;
        difOfThreshold = maxThresholdValue - minThresholdValue;
        
    }

    bool RefreshJointPos()
    {
        bool result = false;
        KinectManager kinectManager = KinectManager.Instance;

        // update Kinect interaction
        if (kinectManager && kinectManager.IsInitialized())
        {
            primaryUserID = kinectManager.GetUserIdByIndex(playerIndex);
            if (primaryUserID != 0)
            {
                result = true;
                // get the left hand state
                leftHandState = kinectManager.GetLeftHandState(primaryUserID);
                leftHandPos = kinectManager.GetJointPosition(primaryUserID, (int)KinectInterop.JointType.HandLeft);

                rightHandState = kinectManager.GetRightHandState(primaryUserID);
                rightHandPos = kinectManager.GetJointPosition(primaryUserID, (int)KinectInterop.JointType.HandRight);

                spineBasePos = kinectManager.GetJointPosition(primaryUserID, (int)KinectInterop.JointType.SpineBase);
                spineShoulderPos = kinectManager.GetJointPosition(primaryUserID, (int)KinectInterop.JointType.SpineShoulder);

                originPos = spineShoulderPos;
                //originPos.y = spineShoulder.y;

            }

        }

        return result;
    }


    float minThresholdValue = 1;
    float maxThresholdValue = 1;
    float difOfThreshold = 1; 

    //以腰为摇杆
    void AnalysePlayerCtrByShouderBase()
    {
        outputStr = "shouder:" + spineShoulderPos*100;
        outputStr += "\nhip:" + spineBasePos * 100;



        outputStr += "\nthresholdValue:" + minThresholdValue;
        outputStr += "\nmaxThresholdValue:" + maxThresholdValue;

        //正常站立识 shoulder和base没在垂直线上，需要补上这个误差  
        float offsetX = spineShoulderPos.x - spineBasePos.x;
        float offsetZ = spineShoulderPos.z  - (spineBasePos.z + OFFSET_Z);
        //考虑到后仰动作难度，将z值放大
        if (offsetZ < 0) offsetZ *= 0.8f;
        else offsetZ = (offsetZ + 0.08f) * 1.5f;

        outputStr += "\noffsetX:" + offsetX;
        outputStr += "\noffsetZ:" + offsetZ;

        float offsetDis = Mathf.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

        float Horizontal = 0;
        float Vertical = 0;
        outputStr += "\noffsetDis:" + offsetDis;

        if (offsetDis > minThresholdValue)
        {
            float sin = offsetX / offsetDis;
            float cos = offsetZ / offsetDis;

            if (offsetDis > maxThresholdValue) offsetDis = maxThresholdValue;
            float scale = (offsetDis - minThresholdValue) / difOfThreshold;

            Horizontal = scale * sin;
            Vertical = scale * cos;

            //过滤误差
            if (Mathf.Abs(Horizontal) < 0.05) Horizontal = 0;
            if (Mathf.Abs(Vertical) < 0.1) Vertical = 0;
        }


        outputStr += "\nHorizontal:" + Horizontal;
        outputStr += "\nVertical:" + Vertical; 
      
        //outputText.text = outputStr;

        GameCtrInput.Instance.CallYStickEvent(Vertical);
        GameCtrInput.Instance.CallBoostEvent(true);

        float minHandPosZ = Mathf.Min(leftHandPos.z, rightHandPos.z);
        if ((spineShoulderPos.z - minHandPosZ) > shoulderBaseDis*0.8) GameCtrInput.Instance.CallShootEvent(true);
    }


    private float minHandDis = 1;

    //双手控制左右 前倾后仰控制拉升和下倾
    void AnalysePlayerCtrByHand()
    {
        float Horizontal = 0;
        float Vertical = 0;
        //hand
        float currntHandDis = Vector3.Distance(leftHandPos, rightHandPos);
        if (currntHandDis > minHandDis)
        {
            float currntSin = (leftHandPos.y - rightHandPos.y) / currntHandDis;
            if (Mathf.Abs(currntSin) > minRecognitionHandSin)
            {
                float RecognitionSin = maxRecognitionHandSin - minRecognitionHandSin;
                currntSin = Mathf.Clamp(currntSin, -RecognitionSin, RecognitionSin);
                Horizontal = currntSin / RecognitionSin;
            }
        }
        GameCtrInput.Instance.CallXStickEvent(Horizontal);
        if (leftHandState == KinectInterop.HandState.Closed || rightHandState == KinectInterop.HandState.Closed)
        {
            GameCtrInput.Instance.CallShootEvent(true);
        }
        outputStr = "leftHandPos:" + leftHandPos * 100;
        outputStr += "\nleftHandPos:" + rightHandPos * 100;
        outputStr += "\nHorizontal:" + Horizontal;
        //outputText.text = outputStr;
    }
}

