using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class ShipCtrl : MonoBehaviour
{
    public float boostAcceleration = 10.0f;
    public float deaccelerationIdle = 0.92f;
    public float deaccelerationBraking = 4f;

    public float normalTurnSpeed = 100.0f;
    public float driftTurnSpeed = 200.0f;
    private float currentTurnSpeed;

    public float driftTiltRate = 4f;
    public float normalTiltRate = 2.5f;
    public float tiltAlignRate = 2f;
    public float driftTiltMax = 70f;
    public float normalTiltMax = 35f;
    private float currentTilt;

    [HideInInspector]
    public float xStick; /* Tilt of left analogue stick every frame. */
    [HideInInspector]
    public float yStick; /* Tilt of left analogue stick every frame. */

    [HideInInspector]
    public bool shooting = false;

    [HideInInspector]
    public bool boosting = false;
    [HideInInspector]
    public bool braking = false;
    [HideInInspector]
    public bool drifting = false;
    [HideInInspector]
    public bool idle = false;
    [HideInInspector]
    public bool swappingWeapon = false;

    [HideInInspector]
    public float currentBoostVelocity = 0;

    public float distanceForwardToCheckForCollision = 1000;
    [HideInInspector]    
    public Vector3 forward;
    [HideInInspector]
    public Vector3 right;

    public float maxBoostVelocity = 150.0f;

    private NetworkIdentity netIndentity;
    void Start()
    {
        netIndentity = GetComponent<NetworkIdentity>();
        
        GameCtrInput.Instance.XStickEvent += HandleXStick;
        GameCtrInput.Instance.YStickEvent += HandleYStick;
        GameCtrInput.Instance.ShootEvent += HandleShoot;
        GameCtrInput.Instance.BoostEvent += HandleBoost;
    }

    void FixedUpdate()
    {
        //if (netIndentity.hasAuthority)

        Rotation();
        Movement();


    }


    void Movement()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        forward = transform.forward;
        /* Adjust velocities based on current spaceship behavior. */
        if (boosting)
        {
            currentBoostVelocity = Mathf.Lerp(currentBoostVelocity, maxBoostVelocity, boostAcceleration * Time.deltaTime);
        }
        else
        {
            currentBoostVelocity = Mathf.Lerp(currentBoostVelocity, 0, deaccelerationIdle * Time.deltaTime);
        }
        currentBoostVelocity = Mathf.Clamp(currentBoostVelocity, 0, maxBoostVelocity);

        Vector3 adjustedForward = forward;

        /* Do some collision detection. If you're about to hit the environment, then adjust for it. */
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, distanceForwardToCheckForCollision))
        {

            if (hit.collider.gameObject.CompareTag("Unpassable"))
            {

                float angleBetweenForwardAndDown = Vector3.Dot(forward, Vector3.down);
                float angleBetweenGroundAndLeft = Vector3.Dot(hit.normal, Vector3.left);

                /* If spaceship is ramming directly into the ground. */
                if (Mathf.Abs(angleBetweenForwardAndDown) <= 1 && Mathf.Abs(angleBetweenGroundAndLeft) <= Mathf.Epsilon)
                {
                    adjustedForward.y = 0.0f;
                }
                /* Otherwise, spaceship is ramming into arbitrary other terrain, eg. wall or incline. */
                else
                {
                    adjustedForward = Vector3.Reflect(adjustedForward, hit.normal);

                    this.GetComponent<Rigidbody>().MoveRotation(
                        Quaternion.Slerp(
                            this.transform.localRotation,
                            Quaternion.Euler(adjustedForward),
                            Time.deltaTime
                        )
                    );
                }
            }
        }

        /* Boost forward. */
        GetComponent<Rigidbody>().MovePosition(
            GetComponent<Rigidbody>().position + Vector3.Slerp(
                Vector3.zero,
                forward * currentBoostVelocity,
                Time.deltaTime
            )
        );

    }


    void Tilt()
    {
       
        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, currentTilt));
    }


    void Rotation()
    {

        currentTurnSpeed = Mathf.Lerp(currentTurnSpeed, normalTurnSpeed, Time.deltaTime * driftTurnSpeed);

        /* Left/right look rotation. */
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles + new Vector3(0.0f, xStick * Time.deltaTime * currentTurnSpeed, 0.0f)
        );

        /* Up/down look rotation. */
        Vector3 newRotationX = transform.localRotation.eulerAngles + new Vector3(-yStick * Time.deltaTime * currentTurnSpeed, 0.0f, 0.0f);
        if (newRotationX.x < 180 && newRotationX.x >= 0)
        {
            newRotationX.x = Mathf.Clamp(newRotationX.x, 0f, 85f);
        }
        else if (newRotationX.x < 0f)
        {
            newRotationX.x = Mathf.Clamp(newRotationX.x, -85f, 0f);
        }
        else
        {
            newRotationX.x = Mathf.Clamp(newRotationX.x, 275f, 360f);
        }

        if (xStick != 0.0f)
        {
            currentTilt = Mathf.Lerp(currentTilt, -xStick * normalTiltMax, driftTiltRate * Time.deltaTime);
        }
        else
        {
            currentTilt = Mathf.Lerp(currentTilt, 0.0f, tiltAlignRate * Time.deltaTime);
        }
        newRotationX.z = currentTilt;
        transform.localRotation = Quaternion.Euler(newRotationX);
    }

    void HandleXStick(float xStick)
    {
        this.xStick = xStick;
       
    }

    void HandleYStick(float yStick)
    {
        this.yStick = yStick;
    }

    void HandleShoot(bool isShoot)
    {
        shooting = isShoot;
    }

    void HandleBoost(bool isBoost)
    {
        boosting = isBoost;
    }


}
