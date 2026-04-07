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
    private AudioSource effectSound;
    public float throwForce = 12f;

    [Header("Ground Check Settings")]
    public float rayHeight = 2f;
    public float rayDistance = 5f;
    public float yOffset = 0f; 

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

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 horizontalMove = direction * moveSpeed;

        Vector3 verticalMove = Vector3.up * verticalVelocity;

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
            //lock XZ position the moment player enters close range
            if (!wasClose)
            {
                lockedPosition = transform.position;
                wasClose = true;
            }

            animator.SetBool("playerIsClose", true);
            ApplyGravityOnly();

            //force XZ position every frame — prevents animation from sliding the enemy
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
        direction.y = 0; 

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

        //tll the ball who its owner is
        BallEffect ballEffect = BlueBall.GetComponent<BallEffect>();
        if (ballEffect != null)
            ballEffect.ownerEnemy = this.gameObject;

        Rigidbody rb = BlueBall.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void ThrowBall()
    {
        if (StorageZone.playerInside) return; //don't throw while player is inside storage house

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
            effectSound = effect.GetComponent<AudioSource>();
        }

    }

    // private void OnDestroy()
    // {
    //     if (spawner != null)
    //         spawner.aliveEnemies--;
    // }

    public void Die()
    {
        if (spawner != null)
        {
            spawner.aliveEnemies--;
            //spawner.aliveEnemies = Mathf.Max(0, spawner.aliveEnemies - 1);
            Debug.Log($"[Enemy] Died, Walking aliveEnemies now: {spawner.aliveEnemies}");
        }
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