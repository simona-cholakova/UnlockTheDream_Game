using UnityEngine;
using System.Collections;

public class BallEffect : MonoBehaviour
{
    public GameObject effect;
    public Transform playerTransform;
    public Transform enemyTransform;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            effect.transform.position = collision.contacts[0].point;
            effect.SetActive(true);

            // Remove any existing follow component first
            FollowPlayer follow = effect.GetComponent<FollowPlayer>();
            if (follow != null)
                Destroy(follow);

            if (PlayerInventory.instance != null && PlayerInventory.instance.shieldActive)
            {
                // Effect stays at impact point when shield is active
                EffectTimer timer = effect.AddComponent<EffectTimer>();
                timer.Begin(enemyTransform, true);
            }
            else
            {
                // Add FollowPlayer component and have it follow the player
                follow = effect.AddComponent<FollowPlayer>();
                follow.target = playerTransform;

                EffectTimer timer = effect.AddComponent<EffectTimer>();
                timer.Begin(null, false);
            }

            gameObject.SetActive(false);
        }
    }
}


public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}


public class EffectTimer : MonoBehaviour
{
    public void Begin(Transform enemy, bool shieldOn)
    {
        StartCoroutine(Run(enemy, shieldOn));
    }

    IEnumerator Run(Transform enemy, bool shieldOn)
    {
        yield return new WaitForSeconds(3f);

        FollowPlayer follow = GetComponent<FollowPlayer>();
        if (follow != null)
        {
            follow.target = null;
            Destroy(follow);
        }

        if (shieldOn && enemy != null)
            Destroy(enemy.gameObject);

        gameObject.SetActive(false);
        Destroy(this);
    }
}