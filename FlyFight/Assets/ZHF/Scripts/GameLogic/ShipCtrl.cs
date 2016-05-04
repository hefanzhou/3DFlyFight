using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class ShipCtrl : NetworkBehaviour
{
    public float boostAcceleration = 10.0f;
    public float maxMoveSpeed = 150.0f;
    public float deaccelerationIdle = 0.92f;

    public float maxHorizontalTurnSpeed = 100.0f;
    public float horizontalTurnSpeed = 200.0f;
    private float currentHorizontalTurnSpeed;

    public float maxVerticalTurnSpeed = 100.0f;
    public float verticalTurnSpeed = 200.0f;
    private float currentVerticalTurnSpeed;

    public float driftTiltRate = 4f;
    public float tiltAlignRate = 2f;
    public float normalTiltMax = 35f;
    private float currentTilt;

    private Transform gunTF;

    public GameObject bulletPrefab;
    [HideInInspector]
    public GamePlayer gamePlayer;

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
    public float currentSpeed = 0;

    public float distanceForwardToCheckForCollision = 1000;
    [HideInInspector]    
    public Vector3 forward;
    [HideInInspector]
    public Vector3 right;

   

    private NetworkIdentity netIndentity;
    void Start()
    {
        netIndentity = GetComponent<NetworkIdentity>();
        gamePlayer = GetComponent<GamePlayer>();
        gunTF = transform.Find("Gun");
        if (netIndentity.hasAuthority) RegisterCtrHandle();

    }

    void RegisterCtrHandle()
    {
        GameCtrInput.Instance.XStickEvent += HandleXStick;
        GameCtrInput.Instance.YStickEvent += HandleYStick;
        GameCtrInput.Instance.ShootEvent += HandleShoot;
        GameCtrInput.Instance.BoostEvent += HandleBoost;

        GameObject.Find("/Group").GetComponent<ShipCamera>().target = gameObject;
        
    }

     [ClientCallback]
    void FixedUpdate()
    {
        if (!netIndentity.hasAuthority || gamePlayer.IsDeath || PVPGameManager.Instance.IsGameOver) return;
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
            currentSpeed = Mathf.Lerp(currentSpeed, maxMoveSpeed, boostAcceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deaccelerationIdle * Time.deltaTime);
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxMoveSpeed);


        /* Boost forward. */
        GetComponent<Rigidbody>().MovePosition(
            GetComponent<Rigidbody>().position + Vector3.Slerp(
                Vector3.zero,
                forward * currentSpeed,
                Time.deltaTime
            )
        );

    }


    void Rotation()
    {

        currentHorizontalTurnSpeed = Mathf.Lerp(currentHorizontalTurnSpeed, maxHorizontalTurnSpeed, Time.deltaTime * horizontalTurnSpeed);

        /* Left/right look rotation. */
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles + new Vector3(0.0f, xStick * Time.deltaTime * currentHorizontalTurnSpeed, 0.0f)
        );
        Vector3 newRotationX;
        float currentVerticalAngle = transform.localRotation.eulerAngles.x;
        if (currentVerticalAngle > 90) currentVerticalAngle -= 360;
        /* Up/down look rotation. */
        if (Mathf.Abs(yStick) >= 0.0001)
        {
            currentVerticalTurnSpeed = Mathf.Lerp(currentVerticalTurnSpeed, maxVerticalTurnSpeed, Time.deltaTime * verticalTurnSpeed);
            currentVerticalAngle += Time.deltaTime * currentVerticalTurnSpeed * -yStick;
        }
        else
        {
            currentVerticalTurnSpeed = 0;
            currentVerticalAngle = Mathf.Lerp(currentVerticalAngle, 0, Time.deltaTime);
        }
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -60, 60);


        if (Mathf.Abs(xStick) >= 0.0001f)
        {
            currentTilt = Mathf.Lerp(currentTilt, -xStick * normalTiltMax, driftTiltRate * Time.deltaTime);
        }
        else
        {
            currentTilt = Mathf.Lerp(currentTilt, 0.0f, tiltAlignRate * Time.deltaTime);
        }
        newRotationX = new Vector3(currentVerticalAngle, transform.localRotation.eulerAngles.y, currentTilt);
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

    private bool onShooting = false;
    private float shootIntervalTime = 0.5f;
    void HandleShoot(bool isShoot)
    {
        shooting = isShoot;
        if (shooting && !onShooting)
        {
            Shoot();
            onShooting = true;
            StartCoroutine(CountTime(shootIntervalTime));
        }
    }

    IEnumerator CountTime(float time)
    {
        yield return new WaitForSeconds(time);
        onShooting = false;
    }

    void HandleBoost(bool isBoost)
    {
        boosting = isBoost;
    }

    void Shoot()
    {
        CmdShoot();
    }

    [Command]
    public void CmdShoot()
    {
        if (!isClient) //avoid to create bullet twice (here & in Rpc call) on hosting client
            CreateBullets();
        //强行在所有客户端生成，避免同步子弹到服务器
        RpcShoot();
    }

    [ClientRpc]
    public void RpcShoot()
    {
        CreateBullets();
    }

    void CreateBullets()
    {
        GameObject go = Instantiate(bulletPrefab, gunTF.position, Quaternion.identity) as GameObject;
        ShipBullet bullet = go.GetComponent<ShipBullet>();
        bullet.originalDirection = transform.forward;
        bullet.owner = gamePlayer;
    }
}
