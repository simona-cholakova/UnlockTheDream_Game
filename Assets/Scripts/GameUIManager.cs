using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    [Header("Keys")]
    public Image[] keySlots;           //3 key icons
    public Color keyLockedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public Color keyUnlockedColor = Color.white;

    [Header("Health")]
    public Slider healthSlider;
    public Image healthFill;
    public Gradient healthGradient;

    [Header("Inventory Icons")]
    public Image shieldIcon;
    public Image potionIcon;
    public Color itemMissingColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    public Color itemHasColor = Color.white;

    [Header("Game Over")]
    public GameObject lostMessage;
    public GameObject gameOverOverlay;
    void Awake() => instance = this;

    void Start()
    {
        //start all keys gray
        foreach (var slot in keySlots)
            slot.color = keyLockedColor;

        //start shield and potion grayed out
        if (shieldIcon != null) shieldIcon.color = itemMissingColor;
        if (potionIcon != null) potionIcon.color = itemMissingColor;

        if (healthSlider != null && PlayerHealth.instance != null)
        {
            healthSlider.maxValue = PlayerHealth.instance.maxHealth;
            healthSlider.value = PlayerHealth.instance.currentHealth;
        }
    }

    //calling this from KeyManager when a key is collected
    public void UpdateKeys(int collectedCount)
    {
        for (int i = 0; i < keySlots.Length; i++)
        {
            keySlots[i].color = i < collectedCount ? keyUnlockedColor : keyLockedColor;
        }
    }
    public void ShowLostMessage()
    {
        if (lostMessage != null)
            lostMessage.SetActive(true);

        if (gameOverOverlay != null)
            gameOverOverlay.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //calling this from PlayerHealth
    public void UpdateHealth(int current)
    {
        if (healthSlider == null) return;
        healthSlider.value = current;

        if (healthFill != null)
            healthFill.color = healthGradient.Evaluate(
                (float)current / PlayerHealth.instance.maxHealth
            );
    }

    //calling these from PlayerInventory when items are picked up
    public void OnShieldPickedUp()
    {
        if (shieldIcon != null) shieldIcon.color = itemHasColor;
    }

    public void OnPotionPickedUp()
    {
        if (potionIcon != null) potionIcon.color = itemHasColor;
    }
}