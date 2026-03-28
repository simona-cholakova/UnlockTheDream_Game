using UnityEngine;

public class KeyPickup : MonoBehaviour
{

    public AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();         
    }
    
    void Update()
    {
        transform.Rotate(0, 0, 100 * Time.deltaTime);
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
