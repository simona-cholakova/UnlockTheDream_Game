using UnityEngine;

public class PotionPickUp : MonoBehaviour
{

    private bool playerClose = false;
    

    void Update()
    {
        if (playerClose && Input.GetKeyDown(KeyCode.C))
        {
            PlayerInventory.instance.PickUpPotion();
            gameObject.SetActive(false);
            Debug.Log("New item to inventory");
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
