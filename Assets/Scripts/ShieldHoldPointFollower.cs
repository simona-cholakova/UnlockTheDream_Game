using UnityEngine;

public class ShieldHoldPointFollower : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform playerCam;

    public Vector3 localOffset = new Vector3(0.5f, -0.3f, 1.5f);

    void LateUpdate()
    {
        // Position: camera world position + offset in camera local space
        transform.position = cameraPosition.position 
            + playerCam.right * localOffset.x 
            + playerCam.up * localOffset.y 
            + playerCam.forward * localOffset.z;

        // Rotation: fixed to camera rotation, no extra spinning
        transform.rotation = playerCam.rotation;
    }
}