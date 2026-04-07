using UnityEngine;

public class StorageDoor : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 200f;

    private bool isOpen;
    private float closedZ;
    private float openZ;

    void Start()
    {
        closedZ = transform.localEulerAngles.z;
        openZ = closedZ + openAngle;
    }

    void Update()
    {
        float targetZ = isOpen ? openZ : closedZ;

        Vector3 euler = transform.localEulerAngles;

        float newZ = Mathf.MoveTowardsAngle(
            euler.z,
            targetZ,
            openSpeed * Time.deltaTime
        );

        transform.localEulerAngles = new Vector3(
            euler.x,
            euler.y,
            newZ
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpen = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpen = false;
    }
}
