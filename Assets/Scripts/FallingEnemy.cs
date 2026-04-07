using UnityEngine;

public class FallingEnemy : MonoBehaviour
{
    public float rotationSpeed = 100f;

    [Header("Sink Settings")]
    public float lifetime = 40f;
    public float sinkDuration = 3f;
    public float sinkSpeed = 2f;

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
        timer += Time.unscaledDeltaTime;

        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        if (!isSinking && timer >= lifetime - sinkDuration)
        {
            StartSinking();
        }

        if (isSinking)
        {
            transform.position += Vector3.down * sinkSpeed * Time.unscaledDeltaTime; //unscaledDeltaTime for something to keep running even if the game is paused
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }

        if (transform.position.y < -50f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit player!");
            PlayerHealth.instance?.TakeDamage(10);
            PlayerCam playerCam = FindObjectOfType<PlayerCam>();

            if (playerCam != null)
            {
                playerCam.PlayHitEffect();
            }
            else
            {
                Debug.LogError("PlayerCam NOT FOUND!");
            }
        }
    }
    void StartSinking()
    {
        isSinking = true;

        if (rb != null)
            rb.isKinematic = true;

        if (col != null)
            col.enabled = false;
    }

    private void OnDestroy()
    {
        FallingEnemySpawner.activeEnemies =
            Mathf.Max(0, FallingEnemySpawner.activeEnemies - 1);
    }
}