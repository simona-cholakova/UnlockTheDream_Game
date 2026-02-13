using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class bigEnemyThrow : MonoBehaviour
{
    public GameObject playerObj;

    private Animator animator;

    public float distanceBetweenObjects;

    [Header("Throw Settings")]
    public GameObject BlueBall;
    public float throwForce = 12f;

    [Header("Ground Check Settings")]
    public float rayHeight = 2f;
    public float rayDistance = 5f;
    public float yOffset = 0f; //set to half collider height MAYBE???

    private float verticalVelocity = 0f;
    private float gravity = -9.81f;


    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 17f;
    CharacterController controller;



    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Vector3 size = rend.bounds.size;
            float width = size.x;
            float height = size.y;

            controller.height = height;
            controller.radius = width / 2f;
            controller.center = new Vector3(0, height / 2f, 0);
        }
    }


    // void MoveTowardsPlayer()
    // {
    //     Vector3 direction = playerObj.transform.position - transform.position;
    //     direction.y = 0;              // ignore height
    //     direction.Normalize();

    //     transform.position += direction * moveSpeed * Time.deltaTime;
    // }

    // void MoveTowardsPlayer()
    // {
    //     Vector3 direction = playerObj.transform.position - transform.position;
    //     direction.y = 0;
    //     direction.Normalize();

    //     Vector3 move = direction * moveSpeed;
    //     move.y += Physics.gravity.y;

    //     controller.Move(move * Time.deltaTime);
    // }

    void MoveTowardsPlayer()
    {
        Vector3 direction = playerObj.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        //horizontal movement
        Vector3 move = direction * moveSpeed;

        //vertical movement
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f; //small value to keep grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }


    private void Update()
    {
        if (playerObj == null) return;

        distanceBetweenObjects = Vector3.Distance(transform.position, playerObj.transform.position);
        Debug.DrawLine(transform.position, playerObj.transform.position, Color.green);
        Debug.Log(distanceBetweenObjects);

        FacePlayer();

        // if (distanceBetweenObjects <= 40f)
        // {
        //     animator.SetBool("playerIsClose", true);
        // }
        // else
        // {
        //     animator.SetBool("playerIsClose", false);
        // }
        if (distanceBetweenObjects > stopDistance)
        {
            animator.SetBool("playerIsClose", false);
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetBool("playerIsClose", true);
        }

    }

    private void FacePlayer()
    {
        //get direction to player (ignore vertical component for ground-based enemies)
        Vector3 direction = playerObj.transform.position - transform.position;
        direction.y = 0; // Keep the enemy upright

        //only rotate if there's a meaningful direction
        if (direction.magnitude > 0.01f)
        {
            //create the rotation to look at player
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = targetRotation;
        }
    }

    public void SpawnBall()
    {
        BlueBall.transform.localPosition = Vector3.zero;
        BlueBall.SetActive(true);

        Rigidbody rb = BlueBall.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void ThrowBall()
    {
        BlueBall.transform.parent = null;

        Rigidbody rb = BlueBall.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 dir = (playerObj.transform.position - BlueBall.transform.position).normalized;
        rb.AddForce(dir * throwForce, ForceMode.Impulse);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (playerObj == null) return;

        Handles.Label(
            (transform.position + playerObj.transform.position) / 2f,
            distanceBetweenObjects.ToString("F2")
        );
    }
#endif
}