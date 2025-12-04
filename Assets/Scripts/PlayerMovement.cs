using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    private bool isGrounded;
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //when touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded!");
            isGrounded = true;
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        //this is how the player always moves relative to where they look
        //orientation.forward points where you look, orientation.right points to your right side
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 targetVelocity = moveDirection.normalized * moveSpeed;

        //get CURRENT velocity
        Vector3 velocity = rb.linearVelocity;

        //keep vertical velocity separate
        float yVel = velocity.y;

        //horizontal velocity only
        Vector3 horizontalVel = new Vector3(velocity.x, 0f, velocity.z);

        //compute change needed
        Vector3 velocityChange = targetVelocity - horizontalVel;

        //clamp max acceleration
        velocityChange = Vector3.ClampMagnitude(velocityChange, acceleration); //limit how much the player's velocity can change

        //if no input decelerate
        if (horizontalInput == 0 && verticalInput == 0)
        {
            Vector3 decel = -horizontalVel * 5f; //adjust "5f" for more or less friction
            rb.AddForce(new Vector3(decel.x, 0f, decel.z), ForceMode.Force);

            //restore vertical velocity and exit early
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, yVel, rb.linearVelocity.z);
            return;
        }

        //apply horizontal force
        rb.AddForce(new Vector3(velocityChange.x, 0f, velocityChange.z), ForceMode.Force);

        //restore vertical velocity so jumping still works
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, yVel, rb.linearVelocity.z);

    }
}
