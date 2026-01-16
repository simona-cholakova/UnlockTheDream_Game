using UnityEngine;
using TMPro;        

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    public int keysCollected = 0;
    public int totalKeys = 3;

    public TMP_Text keyText;     

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddKey()
    {
        keysCollected++;
        UpdateUI();
    }

    void UpdateUI()
    {
        keyText.text = keysCollected + "/" + totalKeys;
    }
}
