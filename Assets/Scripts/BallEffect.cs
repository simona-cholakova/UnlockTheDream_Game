using UnityEngine;
using System.Collections;

public class BallEffect : MonoBehaviour
{
    public GameObject effect;
    public Transform playerTransform;
    public GameObject ownerEnemy;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;

        bool shieldOn = PlayerInventory.instance != null && PlayerInventory.instance.shieldActive;

        if (ownerEnemy != null) Destroy(ownerEnemy);
        else Debug.LogWarning("ownerEnemy is null on ball hit!");

        if (!shieldOn && effect != null && playerTransform != null)
        {
            if (StorageZone.playerInside) return;

            effect.SetActive(true);

            //start coroutine on the effect object itself — survives ball being destroyed
            EffectFollower follower = effect.GetComponent<EffectFollower>();
            if (follower == null)
                follower = effect.AddComponent<EffectFollower>();

            follower.Begin(playerTransform, 1.5f);
        }

        Destroy(gameObject);
    }
}

public class EffectFollower : MonoBehaviour
{
    public void Begin(Transform target, float duration)
    {
        StartCoroutine(Run(target, duration));
    }

    IEnumerator Run(Transform target, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (target != null)
                transform.position = target.position;
            elapsed += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        Destroy(GetComponent<EffectFollower>());
    }
}