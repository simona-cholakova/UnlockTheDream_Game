using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius = 50f;
    public float minDistanceFromPlayer = 50f;
    public float delayBetweenSpawns = 5f;

    [Header("References")]
    public GameObject playerObj;
    public Terrain terrain;

    private int wave = 0;

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        if (enemyPrefab == null || playerObj == null)
        {
            Debug.LogError("Enemy prefab or player reference is missing!");
            yield break;
        }

        // Wave 1 — spawn 1 enemy
        yield return new WaitForSeconds(delayBetweenSpawns);
        SpawnEnemy();

        // Wait for it to die
        yield return new WaitUntil(() =>
            FindObjectsByType<bigEnemyThrow>(FindObjectsSortMode.None).Length == 0);

        // Wave 2+ — spawn 2 enemies with delay between them, then repeat
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(delayBetweenSpawns);
            SpawnEnemy();

            yield return new WaitUntil(() =>
                FindObjectsByType<bigEnemyThrow>(FindObjectsSortMode.None).Length == 0);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        bigEnemyThrow enemyScript = enemy.GetComponent<bigEnemyThrow>();
        if (enemyScript != null)
            enemyScript.playerObj = playerObj;

        Debug.Log("Spawned enemy at " + spawnPosition);
    }

    Vector3 GetRandomSpawnPosition()
    {
        int maxAttempts = 30;

        for (int attempts = 0; attempts < maxAttempts; attempts++)
        {
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            float randomDistance = Random.Range(minDistanceFromPlayer + 2f, spawnRadius);

            Vector3 spawnPosition = playerObj.transform.position + new Vector3(
                Mathf.Cos(randomAngle) * randomDistance,
                0f,
                Mathf.Sin(randomAngle) * randomDistance
            );

            if (terrain != null)
                spawnPosition.y = terrain.SampleHeight(spawnPosition) + terrain.transform.position.y;
            else
                spawnPosition.y = playerObj.transform.position.y;

            if (Vector3.Distance(spawnPosition, playerObj.transform.position) > minDistanceFromPlayer)
                return spawnPosition;
        }

        Debug.LogWarning("Could not find valid spawn position after " + maxAttempts + " attempts");
        return playerObj.transform.position + Vector3.forward * minDistanceFromPlayer;
    }
}