using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class bigEnemyThrow : MonoBehaviour
{
    public GameObject obj;

    private Animator animator;

    public float distanceBetweenObjects;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (obj == null) return;

        distanceBetweenObjects = Vector3.Distance(transform.position, obj.transform.position);
        Debug.DrawLine(transform.position, obj.transform.position, Color.green);
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
        Vector3 direction = obj.transform.position - transform.position;
        direction.y = 0; // Keep the enemy upright
        
        //only rotate if there's a meaningful direction
        if (direction.magnitude > 0.01f)
        {
            //create the rotation to look at player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = targetRotation;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (obj == null) return;

        Handles.Label(
            (transform.position + obj.transform.position) / 2f,
            distanceBetweenObjects.ToString("F2")
        );
    }
#endif
}