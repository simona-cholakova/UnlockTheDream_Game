using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public bool hasShield = false;
    public GameObject equippedShield;
    public bool shieldActive = false;
    public bool hasPotion = false;
    private AudioSource audioSource;

    [Header("Shield Settings")]
    public Transform shieldHoldPoint;
    private float shieldTimer = 0f;
    private bool shieldTimerRunning = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void PickUpShield()
    {
        hasShield = true;
        equippedShield.SetActive(false);
        audioSource.Play();
        Invoke(nameof(StopSound), 1.0f);

        GameUIManager.instance?.OnShieldPickedUp();
    }

    public void PickUpPotion()
    {
        hasPotion = true;

        audioSource.Play();
        Invoke(nameof(StopSound), 1.0f);

        //notify UI
        GameUIManager.instance?.OnPotionPickedUp();
    }

    void StopSound()
    {
        audioSource.Stop();
    }

    public bool IsShieldVisible()
    {
        return hasShield && shieldActive && equippedShield.activeSelf;
    }

    void Update()
    {
        if (hasShield && Input.GetKeyDown(KeyCode.F))
        {
            //always show shield and restart timer on F press
            shieldActive = true;
            equippedShield.SetActive(true);
            shieldTimer = 0f;
            shieldTimerRunning = true;
        }

        if (shieldTimerRunning)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= 7f)
            {
                shieldActive = false;
                equippedShield.SetActive(false);
                shieldTimerRunning = false;
                shieldTimer = 0f;
            }
        }
    }
}