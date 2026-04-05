using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    private bool playerClose = false;

    void Update()
    {
        if (playerClose && Input.GetKeyDown(KeyCode.C))
        {
            PlayerInventory.instance.PickUpShield(); 
            gameObject.SetActive(false);
            Debug.Log("New item added to inventory!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
        }
    }
}
