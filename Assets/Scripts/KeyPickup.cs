using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public int keyID; 
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
            KeyManager.instance.AddKey(keyID); 
            Debug.Log("Key collected: " + keyID);

            audio.Play();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            Destroy(gameObject, audio.clip.length);
        }
    }
}