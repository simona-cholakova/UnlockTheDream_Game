using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        //sets cursor in center and hides it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX; //rotates left/right

        xRotation -= mouseY; //subtract mouseY because moving mouse up means looking up.

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Clamp - not to turn more than 90 degrees
    
        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); //sets the camera's direction.
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); //rotate the player body (only Y-axis)

    }
}
