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
            //place effect at the hit point
            effect.transform.position = collision.contacts[0].point;
            effect.SetActive(true);

            //check if shield is in apllied or not, if yes then attack the enemy, if no then attack player

            if (PlayerInventory.instance != null &&
                PlayerInventory.instance.shieldActive)
            {
                //shield ON, attack enemy
                FollowPlayer follow = effect.GetComponent<FollowPlayer>();

                if (follow == null)
                    follow = effect.AddComponent<FollowPlayer>();

                follow.target = enemyTransform;
                StartCoroutine(DestroyEnemyAfterDelay());

            }
            else
            {
                //shield OFF, attack player
                FollowPlayer follow = effect.GetComponent<FollowPlayer>();

                if (follow == null)
                    follow = effect.AddComponent<FollowPlayer>();

                follow.target = playerTransform;
            }
        }
    }
    IEnumerator DestroyEnemyAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        FollowPlayer follow = effect.GetComponent<FollowPlayer>();

        if (follow != null)
        {
            follow.target = null;
            Destroy(follow);
        }

        if (enemyTransform != null)
        {
            Destroy(enemyTransform.gameObject);
        }

        effect.SetActive(false);
    }

}


//helper script to make the effect follow the player
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
