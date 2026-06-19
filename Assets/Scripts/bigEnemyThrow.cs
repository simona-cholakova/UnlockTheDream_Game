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
        animator.applyRootMotion = false; //disable animation movement so character is moved only by code
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = playerObj.transform.position - transform.position; //vector pointing from enemy to player
        direction.y = 0; //ignore vertical difference 
        direction.Normalize(); //vector length = 1
        //speed is controlled only by moveSpeed not distance

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; //makes enemy fall down kinda naturally 
        }

        Vector3 horizontalMove = direction * moveSpeed; //enemy moves toward player at moveSpeed

        Vector3 verticalMove = Vector3.up * verticalVelocity; //jumping/falling 

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
        Vector3 direction = playerObj.transform.position - transform.position;
        direction.y = 0; 

        //only rotate if there's a meaningful direction
        if (direction.magnitude > 0.01f)
        {
            //create the rotation to look at player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime); //smoothly rotates

            transform.rotation = targetRotation;
        }
    }

    public void SpawnBall() //called by animation marker
    {
        BlueBall.transform.localPosition = Vector3.zero;
        BlueBall.SetActive(true);

        //tell the ball who its owner is
        BallEffect ballEffect = BlueBall.GetComponent<BallEffect>();
        if (ballEffect != null)
            ballEffect.ownerEnemy = this.gameObject;

        Rigidbody rb = BlueBall.GetComponent<Rigidbody>();
        rb.isKinematic = true; //stops physics
        rb.useGravity = false;
    }

    public void ThrowBall() //called by animation marker
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

    public void Die()
    {
        if (spawner != null)
        {
            spawner.aliveEnemies--;
            //Debug.Log($"[Enemy] Died, Walking aliveEnemies now: {spawner.aliveEnemies}");
        }
        Destroy(gameObject);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos() //for distance in editor
    {
        if (playerObj == null) return;

        Handles.Label(
            (transform.position + playerObj.transform.position) / 2f,
            distanceBetweenObjects.ToString("F2")
        );
    }
#endif
}