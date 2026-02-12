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


    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerObj == null) return;

        distanceBetweenObjects = Vector3.Distance(transform.position, playerObj.transform.position);
        Debug.DrawLine(transform.position, playerObj.transform.position, Color.green);
        Debug.Log(distanceBetweenObjects);

        FacePlayer();

        if (distanceBetweenObjects <= 40f)
        {
            animator.SetBool("playerIsClose", true);
        }
        else
        {
            animator.SetBool("playerIsClose", false);
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