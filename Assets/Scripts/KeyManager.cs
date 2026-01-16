using UnityEngine;
using TMPro;        // use this if TextMeshPro
// using UnityEngine.UI; // use this if normal UI Text

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    public int keysCollected = 0;
    public int totalKeys = 3;

    public TMP_Text keyText;     // TextMeshPro
    // public Text keyText;      // Normal UI Text version

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
