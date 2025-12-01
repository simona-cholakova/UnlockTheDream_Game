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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //stops the player from tipping over when hitting walls
    }

    void Update()
    {
        MyInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //orientation.forward points where you look, orientation.right points to your right side
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
