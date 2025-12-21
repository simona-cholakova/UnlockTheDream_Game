// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Movement")]
//     public float moveSpeed;
//     public float groundDrag;
//     public float jumpForce;
//     public float jumpCooldown;
//     public float airMultiplier;
//     bool readyToJump;

//     public Transform orientation; //transform that always faces same horizontal direction as your camera, makes movement follow where you look
//     float horizontalInput;
//     float verticalInput;
//     Vector3 moveDirection;
//     Rigidbody rb;

//     [Header("Keybinds")]
//     public KeyCode jumpKey = KeyCode.Space;

//     [Header("Ground Check")]
//     public float playerHeight;
//     public LayerMask groundLayer;
//     bool isGrounded;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>(); //gets the Rigidbody that is attached to the player
//         rb.freezeRotation = true; //stops the player from tipping over when hitting walls
//         readyToJump = true;
//     }

//     //use Update() for input and use FixedUpdate() for physics

//     void Update()
//     {
//         //ground check 
//         //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);
//         // float groundCheckDistance = 0.4f;
//         // float radius = 0.3f;
//         // isGrounded = Physics.SphereCast(transform.position, radius, Vector3.down,
//         //                                 out RaycastHit hit, groundCheckDistance, groundLayer);

//         // Check what the actual distance to ground would be
//         float currentHeight = transform.position.y;
//         Debug.Log("Current Height: " + currentHeight);

//         // Make raycast distance much larger to reach the ground
//         float raycastDistance = currentHeight + 5f; // Add extra buffer

//         // Ground check with longer distance
//         isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);

//         // Visualize the ray
//         Debug.DrawRay(transform.position, Vector3.down * raycastDistance,
//                       isGrounded ? Color.green : Color.red);

//         Debug.Log("Is Grounded: " + isGrounded + " | Ray Distance: " + raycastDistance);

//         MyInput();
//         speedControl();

//         //handle drag
//         if (isGrounded)
//         {
//             rb.linearDamping = groundDrag;
//         }
//         else
//         {
//             rb.linearDamping = 0;
//         }

//     }

//     void FixedUpdate() //unity runs this at a fixed rate
//     {
//         MovePlayer();
//     }

//     private void MyInput()
//     {
//         horizontalInput = Input.GetAxisRaw("Horizontal");
//         verticalInput = Input.GetAxisRaw("Vertical");

//         //when to jump
//         if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
//         {
//             readyToJump = false;
//             Jump();
//             Invoke(nameof(resetJump), jumpCooldown); //when space remains pressed, you keep jumping
//         }
//     }


//     private void MovePlayer()
//     {
//         //calculate movement direction
//         //this is how the player always moves relative to where they look
//         //orientation.forward points where you look, orientation.right points to your right side
//         moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

//         if (isGrounded) //on ground
//         {
//             rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
//         }
//         else if (!isGrounded) //in air
//         {
//             rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
//         }

//     }
//     private void speedControl()
//     {
//         Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

//         //limit velocity if needed 
//         if (flatVelocity.magnitude > moveSpeed)
//         {
//             Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
//             rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
//         }
//     }

//     private void Jump()
//     {
//         //reset y velocity to 0, so you always jump the same height 
//         rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

//         rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //impulse because you are only applying the force once
//     }

//     private void resetJump()
//     {
//         readyToJump = true;
//     }
// }


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    public Transform orientation;
    public KeyCode jumpKey = KeyCode.Space;

    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // keeps player grounded
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move =
            orientation.forward * vertical +
            orientation.right * horizontal;

        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
