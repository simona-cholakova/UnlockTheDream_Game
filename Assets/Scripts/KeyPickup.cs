using UnityEngine;

public class KeyPickup : MonoBehaviour
{

    public AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();         
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyManager.instance.AddKey();
            audio.Play();
            Debug.Log("Key collected!");
        }
    }
}
