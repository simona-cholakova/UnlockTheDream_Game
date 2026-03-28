using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public bool hasShield = false;
    public GameObject equippedShield;
    public bool shieldActive = false;

    [Header("Shield Settings")]
    public Transform shieldHoldPoint;
    // Adjust these values in the Inspector to get the exact look from your screenshot
    public Vector3 shieldOffset = new Vector3(0.5f, -0.4f, 1.2f);
    public Vector3 shieldRotation = new Vector3(0, 0, 0);

    void Awake() => instance = this;

    public void PickUpShield()
    {
        hasShield = true;

        // 1. Move it to the camera holder
        equippedShield.transform.SetParent(shieldHoldPoint);

        equippedShield.transform.localPosition = shieldOffset;
        equippedShield.transform.localRotation = Quaternion.Euler(shieldRotation);

        // 4. FORCE SCALE: Make sure it's not giant or tiny
        //equippedShield.transform.localScale = new Vector3(1f, 1f, 1f);

        equippedShield.SetActive(false);
    }

    void Update()
    {
        // Toggle shield visibility
        if (hasShield && Input.GetKeyDown(KeyCode.F))
        {
            shieldActive = !shieldActive;
            equippedShield.SetActive(shieldActive);
        }
    }
}