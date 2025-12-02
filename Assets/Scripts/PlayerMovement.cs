using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation; //transform that always faces same horizontal direction as your camera, makes movement follow where you look
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    public float acceleration = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //gets the Rigidbody that is attached to the player
        rb.freezeRotation = true; //stops the player from tipping over when hitting walls
    }

    //use Update() for input and use FixedUpdate() for physics

    void Update()
    {
        MyInput();
    }

    void FixedUpdate() //unity runs this at a fixed rate
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        //calculate movement direction
        //this is how the player always moves relative to where they look
        //orientation.forward points where you look, orientation.right points to your right side
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 targetVelocity = moveDirection.normalized * moveSpeed;

        Vector3 velocity = rb.linearVelocity;
        Vector3 horizontalVel = new Vector3(velocity.x, 0f, velocity.z);

        Vector3 velocityChange = targetVelocity - horizontalVel;

        // limit max change per frame
        velocityChange = Vector3.ClampMagnitude(velocityChange, acceleration);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);


    }
}
