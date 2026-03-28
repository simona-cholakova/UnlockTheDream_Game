using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class bigEnemyThrow : MonoBehaviour
{
    public GameObject playerObj;

    private Animator animator;

    public float distanceBetweenObjects;
    public EnemySpawner spawner;

    [Header("Throw Settings")]
    public GameObject BlueBall;
    public GameObject throwEffect;
    public float throwForce = 12f;

    [Header("Ground Check Settings")]
    public float rayHeight = 2f;
    public float rayDistance = 5f;
    public float yOffset = 0f; //set to half collider height MAYBE???

    private float verticalVelocity = 0f;
    private float gravity = -25f;


    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 17f;
    CharacterController controller;

    private Vector3 lockedPosition;
    private bool wasClose = false;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator.applyRootMotion = false;
    }


    void MoveTowardsPlayer()
    {
        Vector3 direction = playerObj.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        // Handle grounding / gravity
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Horizontal movement (world units/sec → scale by deltaTime in Move)
        Vector3 horizontalMove = direction * moveSpeed;

        // Vertical movement: velocity is already units/sec, so scale by deltaTime here
        Vector3 verticalMove = Vector3.up * verticalVelocity;

        // Combine and move — both components correctly scaled once
        controller.Move((horizontalMove + verticalMove) * Time.deltaTime);
    }

    private void Update()
    {
        if (playerObj == null) return;

        distanceBetweenObjects = Vector3.Distance(transform.position, playerObj.transform.position);
        Debug.DrawLine(transform.position, playerObj.transform.position, Color.green);

        FacePlayer();

        bool isClose = distanceBetweenObjects <= stopDistance;

        if (!isClose)
        {
            wasClose = false;
            animator.SetBool("playerIsClose", false);
            MoveTowardsPlayer();
        }
        else
        {
            // Lock XZ position the moment player enters close range
            if (!wasClose)
            {
                lockedPosition = transform.position;
                wasClose = true;
            }

            animator.SetBool("playerIsClose", true);
            ApplyGravityOnly();

            // Force XZ position every frame — prevents animation from sliding the enemy
            transform.position = new Vector3(
                lockedPosition.x,
                transform.position.y,
                lockedPosition.z
            );
        }
    }

    void ApplyGravityOnly()
    {
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            transform.rotation = targetRotation;
        }
    }
    // bool IsGrounded()
    // {
    //     Vector3 origin = transform.position + Vector3.up * 0.2f;
    //     return Physics.Raycast(origin, Vector3.down, 0.4f);
    // }


    public void SpawnBall()
    {
        BlueBall.transform.localPosition = Vector3.zero;
        BlueBall.SetActive(true);

        // Tell the ball who its owner is
        BallEffect ballEffect = BlueBall.GetComponent<BallEffect>();
        if (ballEffect != null)
            ballEffect.ownerEnemy = this.gameObject;

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

        if (throwEffect != null)
        {
            GameObject effect = Instantiate(throwEffect, BlueBall.transform.position, Quaternion.identity);
            effect.SetActive(true);
        }

        // ← Destroy(gameObject, 0.5f) REMOVED — BallEffect handles cleanup now
    }

    public void Die()
    {
        if (spawner != null)
            spawner.aliveEnemies--;

        Destroy(gameObject);
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