using Mono.Cecil;
using UnityEngine;

public class FallingEnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject fallingEnemy;
    public float spawnInterval = 5f;
    public float spawnHeight = 10f;
    public int maxActiveEnemies = 5;

    [Header("Spawn Area")]
    public float spawnRadius = 100f;

    [Header("References")]
    public Terrain terrain;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f; //reset the timer

            FallingEnemy[] active = FindObjectsByType<FallingEnemy>(FindObjectsSortMode.None);
            if (active.Length < maxActiveEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        //random position within a circle around this spawner object
        float angle = Random.Range(0f, Mathf.PI * 2f); //picks a random angle in radians
        float distance = Random.Range(0f, spawnRadius); //random distance from the center

        Vector3 spawnPos = transform.position + new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );

        spawnPos.y = terrain.SampleHeight(spawnPos) + spawnHeight;

        Instantiate(fallingEnemy, spawnPos, Quaternion.identity);

    }
}
