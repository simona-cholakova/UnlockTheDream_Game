using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius = 520f;
    public float minDistanceFromPlayer = 50f;
    public float delayBetweenSpawns = 4f;
    public int aliveEnemies = 0;

    [Header("References")]
    public GameObject playerObj;
    public Terrain terrain;

    [Header("Ball References")]

    public GameObject hitEffect;

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    void SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    IEnumerator WaveLoop()
    {
        //Debug.Log("Continuous spawning started");
        while (true)
        {
            SpawnWave(3); //number of enemies per wave

            yield return new WaitForSeconds(16f); //delay between waves, pause this coroutine for 16 seconds then continue
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        if (StorageZone.playerInside)
        {
            //Debug.Log("Player inside storage, enemy removed");
            Destroy(enemy);
            return; 
        }

        aliveEnemies++;

        bigEnemyThrow enemyScript = enemy.GetComponent<bigEnemyThrow>();
        if (enemyScript != null)
        {
            enemyScript.playerObj = playerObj;
            enemyScript.spawner = this;

            if (enemyScript.BlueBall != null)
            {
                BallEffect ballEffect = enemyScript.BlueBall.GetComponent<BallEffect>();
                if (ballEffect != null)
                {
                    ballEffect.playerTransform = playerObj.transform;
                    ballEffect.effect = hitEffect;
                }
            }
        }
        //Debug.Log("Spawned enemy at " + spawnPosition);
    }

    Vector3 GetRandomSpawnPosition()
    {
        int maxAttempts = 50;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(0, terrain.terrainData.size.x);
            float z = Random.Range(0, terrain.terrainData.size.z);

            Vector3 worldPos = new Vector3(
                x + terrain.transform.position.x,
                0,
                z + terrain.transform.position.z
            );

            worldPos.y = terrain.SampleHeight(worldPos) + terrain.transform.position.y; //for enemy to spawn on top of the terrain

            if (Vector3.Distance(worldPos, playerObj.transform.position) < minDistanceFromPlayer)
                continue;

            if (IsInsideStorage(worldPos))
                continue;

            return worldPos;
        }
        return playerObj.transform.position + Random.insideUnitSphere * minDistanceFromPlayer; //if normal spawning fails spawn in radius of 1 from player
    }

    bool IsInsideStorage(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, 1f); //check for colliders in radius 1 around the position to see if it's inside the storage house 

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Storage"))
                return true;
        }

        return false;
    }

    // void Update()
    // {
    //     Debug.Log($"[Spawner] WalkingaliveEnemies: {aliveEnemies}");
    // }
}