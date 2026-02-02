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
        if (distanceBetweenObjects <= 35f)
        {
            animator.SetBool("playerIsClose", true);
        }
        else
        {
            animator.SetBool("playerIsClose", false);
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