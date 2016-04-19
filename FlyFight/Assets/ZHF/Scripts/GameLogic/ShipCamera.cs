using UnityEngine;
using System.Collections;

public class ShipCamera : MonoBehaviour {

    public GameObject target;
    public Vector3 cameraToModel = new Vector3(0, 0, -100);
   
    public float rotationSpeed = 3.0f;
    public float lookSpeed = 0.7f;


    void LateUpdate()
    {
        Vector3 forward = target.transform.forward; 

        Vector3 newCameraPosition = target.transform.position + forward * cameraToModel.z;
        newCameraPosition.x += cameraToModel.x;
        newCameraPosition.y += cameraToModel.y;

        this.transform.position = Vector3.Lerp(
            this.transform.position,
            newCameraPosition,
            Mathf.Clamp01(rotationSpeed * Time.deltaTime)
            );

        Vector3 lookPoint = target.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Clamp01(lookSpeed * Time.deltaTime));


    }
	
	
}
