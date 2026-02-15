using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public bool hasShield = false;
    public GameObject equippedShield;
    private bool shieldActive = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(hasShield && Input.GetKeyDown(KeyCode.F))
        {
            shieldActive = !shieldActive;
            equippedShield.SetActive(shieldActive);
        }
    }
    
}
