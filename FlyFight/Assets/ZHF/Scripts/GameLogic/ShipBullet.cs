using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShipBullet : MonoBehaviour {

    public Vector3 originalDirection;

    //The spaceship that shoot that bullet, use to attribute point correctly
    public GamePlayer owner;
    public int harm = 1;

    void Start()
    {
        Destroy(gameObject, 2.0f);
        GetComponent<Rigidbody>().velocity = originalDirection * 6000.0f;
        transform.forward = originalDirection;
    }

    void OnCollisionEnter(Collision collision)
    {
        GamePlayer gamePlayer = collision.gameObject.GetComponent<GamePlayer>();
        if (gamePlayer != null && gamePlayer != owner)
        {
            PVPGameManager.Instance.CalcDemage(gamePlayer, this);
            Destroy(gameObject);
        }
    }

   

}
