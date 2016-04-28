using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class KinectCtrPlayerInput : MonoBehaviour {
    [Range(0.1f, 0.8f)]
    public float minRecognitionSin = 0.2f;
    [Range(0.1f, 0.8f)]
    public float maxRecognitionSin = 0.5f;
    [Range(1, 1.5f)]
    public float blowUpZValue = 1.2f;
    private const float OFFSET_Z = -0.07f;
	// Use this for initialization
    private KinectInputModule kinectInput;
	void Start () {
       

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
        if (RefreshJointPos())
        {
            AnalysePlayerCtr();
        }
        
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

   
    void AnalysePlayerCtr()
    {
        outputStr = "shouder:" + spineShoulderPos*100;
        outputStr += "\nhip:" + spineBasePos * 100;

        shoulderBaseDis = Vector3.Distance(spineShoulderPos, spineBasePos);
        float minThresholdValue = shoulderBaseDis * minRecognitionSin;
        float maxThresholdValue = shoulderBaseDis * maxRecognitionSin;
        float difOfThreshold = maxThresholdValue - minThresholdValue;

        outputStr += "\nthresholdValue:" + minThresholdValue;
        outputStr += "\nmaxThresholdValue:" + maxThresholdValue;

        //正常站立识 shoulder和base没在垂直线上，需要补上这个误差  
        float offsetX = spineShoulderPos.x - spineBasePos.x;
        float offsetZ = spineShoulderPos.z - spineBasePos.z + OFFSET_Z;
        //考虑到前倾动作难度，将z值放大
        offsetZ *= blowUpZValue;
        
        float offsetDis = Mathf.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

        float Horizontal = 0;
        float Vertical = 0;

        if (offsetDis > minThresholdValue)
        {
            outputStr += "\noffsetDis:" + offsetDis;
            if (offsetDis > maxThresholdValue) offsetDis = maxThresholdValue;
            float factor = (1 - minThresholdValue / offsetDis);
            offsetX *= factor;
            offsetZ *= factor;

            Horizontal = offsetX / difOfThreshold;
            Vertical = offsetZ / difOfThreshold;

            if (Mathf.Abs(Horizontal) > 1)
            {
                Debug.LogError("out");
            }
            //过滤误差
            if (Mathf.Abs(Horizontal) < 0.05) Horizontal = 0;
            if (Mathf.Abs(Vertical) < 0.05) Vertical = 0;
        }

        outputStr += "\noffsetX:" + offsetX;
        outputStr += "\noffsetZ:" + offsetZ;

        outputStr += "\nHorizontal:" + Horizontal;
        outputStr += "\nVertical:" + Vertical; 
      
        outputText.text = outputStr;

        GameCtrInput.Instance.CallXStickEvent(Horizontal);
        GameCtrInput.Instance.CallYStickEvent(Vertical);
        GameCtrInput.Instance.CallBoostEvent(true);
      
    }


}


//part1 用手控制
//Vector3 leftOffest = leftHandPos - originPos;
//leftOffest.x *= -1;
//leftOffest.z *= -1;

//Vector3 rightOffest = rightHandPos - originPos;
//rightOffest.z *= -1;



//outputStr = thresholdValue + "\nright:" + rightOffest;
//if (rightOffest.y > thresholdValue) outputStr += "\nY";
//else if (rightOffest.x > thresholdValue) outputStr += "-X";
//else if (rightOffest.z > thresholdValue) outputStr += "-Z";

//outputStr += "\nleft:" + leftOffest;
//if (leftOffest.y > thresholdValue) outputStr += "\nY";
//else if (leftOffest.x > thresholdValue) outputStr += "-X";
//else if (leftOffest.z > thresholdValue) outputStr += "-Z";