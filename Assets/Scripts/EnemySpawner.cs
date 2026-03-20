using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public int maxActiveEnemies = 1;
    public float spawnRadius = 50f;
    public float minDistanceFromPlayer = 50f;
    public float delayBetweenSpawns = 5f;

    [Header("References")]
    public GameObject playerObj;
    public Terrain terrain;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRandomly());
    }

    System.Collections.IEnumerator SpawnEnemiesRandomly()
    {
        if (enemyPrefab == null || playerObj == null)
        {
            Debug.LogError("Enemy prefab or player reference is missing!");
            yield break;
        }

        while (true)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            
            // Instantiate the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            // Assign the player reference to the enemy script
            bigEnemyThrow enemyScript = enemy.GetComponent<bigEnemyThrow>();
            if (enemyScript != null)
            {
                enemyScript.playerObj = playerObj;
            }

            Debug.Log("Spawned enemy at " + spawnPosition);

            // Wait before spawning the next enemy
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;
        int attempts = 0;
        int maxAttempts = 30;

        while (!validPosition && attempts < maxAttempts)
        {
            attempts++;
            
            // Random point in a circle around the player (not spawner)
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float randomDistance = Random.Range(minDistanceFromPlayer + 2f, spawnRadius);
            
            spawnPosition = playerObj.transform.position + new Vector3(
                Mathf.Cos(randomAngle) * randomDistance,
                0,
                Mathf.Sin(randomAngle) * randomDistance
            );

            // Check distance from player
            if (Vector3.Distance(spawnPosition, playerObj.transform.position) > minDistanceFromPlayer)
            {
                validPosition = true;
            }

            // Get height from terrain if available
            if (terrain != null)
            {
                spawnPosition.y = terrain.SampleHeight(spawnPosition) + terrain.transform.position.y;
            }
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Could not find valid spawn position after " + maxAttempts + " attempts");
        }

        return spawnPosition;
    }
}
