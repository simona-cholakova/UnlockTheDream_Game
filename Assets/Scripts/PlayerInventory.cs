using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public bool hasShield = false;

    void Awake()
    {
        instance = this;
    }
    
}
