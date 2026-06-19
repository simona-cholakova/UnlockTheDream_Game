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

        bool shieldOn = PlayerInventory.instance != null && PlayerInventory.instance.IsShieldVisible();

        if (ownerEnemy != null)
        {
            bigEnemyThrow enemyScript = ownerEnemy.GetComponent<bigEnemyThrow>();

            if (enemyScript != null)
            {
                enemyScript.Die(); //destroy enemy 
            }
            else
            {
                Debug.LogWarning("bigEnemyThrow not found on ownerEnemy!");
                Destroy(ownerEnemy); //destroy enemy 
            }
        }
        else
        {
            Debug.LogWarning("ownerEnemy is null on ball hit!");
        }

        if (shieldOn)
        {
            Debug.Log("TRYING TO COUNT BLOCK");
        }

        if (!shieldOn && effect != null && playerTransform != null)
        {
            PlayerHealth.instance?.TakeDamage(20);

            if (StorageZone.playerInside) return;

            effect.SetActive(true);

            EffectFollower follower = effect.GetComponent<EffectFollower>();
            if (follower == null)
                follower = effect.AddComponent<EffectFollower>();

            follower.Begin(playerTransform, 1.5f); //(target, duration) 
        }

        Destroy(gameObject); //destroy ball after ground hit
    }

}

//for effect to follow the player 
public class EffectFollower : MonoBehaviour
{
    public void Begin(Transform target, float duration) //coroutine (a timed loop that runs over multiple frames)
    {
        StartCoroutine(Run(target, duration));
    }

    IEnumerator Run(Transform target, float duration) //coroutine runs every frame
    {
        float effectActiveTime = 0f;

        while (effectActiveTime < duration)
        {
            if (target != null)
                transform.position = target.position;

            effectActiveTime += Time.deltaTime;
            yield return null; //pause here and continue in next frame
        }

        gameObject.SetActive(false);
        Destroy(GetComponent<EffectFollower>());
    }
}