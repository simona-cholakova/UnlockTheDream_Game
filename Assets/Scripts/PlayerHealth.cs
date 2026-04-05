using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public int maxHealth = 100;
    public int currentHealth;

    public UnityEvent onDeath;
    public UnityEvent<int> onHealthChanged; // passes current health

    void Awake()
    {
        instance = this;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        GameUIManager.instance?.UpdateHealth(currentHealth); 
        if (currentHealth <= 0) onDeath?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(currentHealth);
    }
}