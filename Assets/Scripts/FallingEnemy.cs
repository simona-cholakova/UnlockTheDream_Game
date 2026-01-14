using UnityEngine;

public class FallingEnemy : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Falling object hit the player!");
        }
    }
}
