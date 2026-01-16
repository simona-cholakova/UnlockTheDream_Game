using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyManager.instance.AddKey();
            Debug.Log("Key collected!");
        }
    }
}
