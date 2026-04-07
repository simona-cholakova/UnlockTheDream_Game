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
        //Debug.Log("Something entered trigger: " + other.gameObject.name + " tag: " + other.gameObject.tag);

        if (other.CompareTag("Player"))
        {
            KeyManager.instance.AddKey(keyID);
            //Debug.Log("Key collected: " + keyID);

            if (audio != null && audio.clip != null)
            {
                audio.Play();
                Destroy(gameObject, audio.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }

            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}