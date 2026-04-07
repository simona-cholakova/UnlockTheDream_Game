using UnityEngine;

public class StorageZone : MonoBehaviour
{
    public static bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            //Debug.Log("Player entered storage");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            //Debug.Log("Player exited storage");
        }
    }
}