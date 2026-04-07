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
    //public Vector3 shieldOffset = new Vector3(0.5f, -0.4f, 1.2f);
    //public Vector3 shieldRotation = new Vector3(0, 0, 0);

    private float shieldTimer = 0f;
    private bool shieldTimerRunning = false;

    void Awake() => instance = this;

    // public void PickUpShield()
    // {
    //     hasShield = true;
    //     equippedShield.transform.SetParent(shieldHoldPoint);
    //     equippedShield.transform.localPosition = shieldOffset;
    //     equippedShield.transform.localRotation = Quaternion.Euler(shieldRotation);
    //     equippedShield.SetActive(false);

    //     //notify UI
    //     GameUIManager.instance?.OnShieldPickedUp();
    // }

    public void PickUpShield()
    {
        hasShield = true;
        equippedShield.transform.SetParent(shieldHoldPoint);
        equippedShield.transform.localPosition = Vector3.zero;
        equippedShield.transform.localRotation = Quaternion.identity;
        equippedShield.SetActive(false);

        GameUIManager.instance?.OnShieldPickedUp();
    }

    public void PickUpPotion()
    {
        hasPotion = true;

        //notify UI
        GameUIManager.instance?.OnPotionPickedUp();
    }

    public bool IsShieldVisible()
    {
        return hasShield && shieldActive && equippedShield.activeSelf;
    }

    void Update()
    {
        if (hasShield && Input.GetKeyDown(KeyCode.F))
        {
            //Always show shield and restart timer on F press
            shieldActive = true;
            equippedShield.SetActive(true);
            shieldTimer = 0f;
            shieldTimerRunning = true;
        }

        if (shieldTimerRunning)
        {
            shieldTimer += Time.deltaTime;
            if(shieldTimer >= 7f)
            {
                shieldActive = false;
                equippedShield.SetActive(false);
                shieldTimerRunning = false;
                shieldTimer = 0f;
            }
        }
    }
}