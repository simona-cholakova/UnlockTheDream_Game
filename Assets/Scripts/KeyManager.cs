using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    public int keysCollected = 0;
    public int totalKeys = 3;

    public TMP_Text keyText;
    public GameObject crystalParent;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddKey(int keyID)
    {
        keysCollected++;
        UpdateUI();

        if (keyID == 2)
            HideCrystals();
    }

    void UpdateUI()
    {
        keyText.text = keysCollected + "/" + totalKeys;
    }
    void HideCrystals()
    {
        if (crystalParent != null)
            crystalParent.SetActive(false);
    }
}
