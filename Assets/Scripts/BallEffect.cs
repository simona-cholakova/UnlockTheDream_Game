using UnityEngine;

public class BallEffect : MonoBehaviour
{
    public GameObject effect;
    public Transform playerTransform;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Place effect at the hit point first
            effect.transform.position = collision.contacts[0].point;
            effect.SetActive(true);

            // Start following the player
            effect.AddComponent<FollowPlayer>().target = playerTransform;

            //Destroy(gameObject);
        }
    }
}

// This helper script makes the effect follow the player
public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    private void Update()
    {
        if (target != null)
        {
            // Smoothly follow the player
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
