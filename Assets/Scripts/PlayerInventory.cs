using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public bool hasShield = false;
    public GameObject equippedShield;
    public bool shieldActive = false;
    public bool hasPotion = false;

    [Header("Shield Settings")]
    public Transform shieldHoldPoint;
    public Vector3 shieldOffset = new Vector3(0.5f, -0.4f, 1.2f);
    public Vector3 shieldRotation = new Vector3(0, 0, 0);

    void Awake() => instance = this;

    public void PickUpShield()
    {
        hasShield = true;

        equippedShield.transform.SetParent(shieldHoldPoint);

        equippedShield.transform.localPosition = shieldOffset;
        equippedShield.transform.localRotation = Quaternion.Euler(shieldRotation);

        equippedShield.SetActive(false);
    }

    public void PickUpPotion()
    {
        hasPotion = true;
    }

    void Update()
    {
        if (hasShield && Input.GetKeyDown(KeyCode.F))
        {
            shieldActive = !shieldActive;
            equippedShield.SetActive(shieldActive);
        }
    }
}