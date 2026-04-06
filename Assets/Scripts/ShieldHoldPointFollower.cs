using UnityEngine;

public class ShieldHoldPointFollower : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform playerCam;
    public Vector3 localOffset = new Vector3(0.5f, -0.3f, 2.5f);

    void LateUpdate()
    {
        transform.position = cameraPosition.position 
            + playerCam.right * localOffset.x 
            + playerCam.up * localOffset.y 
            + playerCam.forward * localOffset.z;

        transform.rotation = playerCam.rotation;
    }
}