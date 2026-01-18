using UnityEngine;

public class FallingEnemy : MonoBehaviour
{

    public ParticleSystem myParticles;
    public float rotationSpeed = 100f;

    void Update()
    {
        //rotate Y axis while falling
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player!");
            myParticles.Play();
        }
    }
}
