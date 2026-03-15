using UnityEngine;

public class FallingEnemy : MonoBehaviour
{
    public ParticleSystem myParticles;
    public float rotationSpeed = 100f;

    [Header("Sink Settings")]
    public float lifetime = 60f;        //total time (falling+sinking) before gone
    public float sinkDuration = 3f;     //how long the sinking takes
    public float sinkSpeed = 2f;        //how fast it sinks into ground

    private float timer = 0f;
    private bool isSinking = false;
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        //always rotate on Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        //start sinking when lifetime is nearly up
        if (!isSinking && timer >= lifetime - sinkDuration)
        {
            StartSinking();
        }

        // Sink into the ground
        if (isSinking)
        {
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
        }

        // Hard destroy fallback after full lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player!");
            myParticles.Play();
        }
    }

    void StartSinking()
    {
        isSinking = true;

        // Disable physics and collider so it slides through the ground cleanly
        rb.isKinematic = true;
        col.enabled = false;
    }
}