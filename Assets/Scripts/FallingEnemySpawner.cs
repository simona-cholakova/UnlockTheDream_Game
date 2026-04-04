using UnityEngine;

public class FallingEnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject fallingEnemy;
    public float spawnInterval = 5f;
    public float spawnHeight = 200f;
    public int maxActiveEnemies = 5;

    [Header("Spawn Area")]
    public float spawnRadius = 1000f;

    [Header("References")]
    public Terrain terrain;

    private float timer = 0f;

    public static int activeEnemies = 0;

    void Awake()
    {
        activeEnemies = 0;
        Debug.Log("[Spawner] Awake — activeEnemies reset to 0");
    }

    void OnEnable()
    {
        activeEnemies = 0;
        Debug.Log("[Spawner] OnEnable — activeEnemies reset to 0");
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            Debug.Log($"[Spawner] Tick — activeEnemies: {activeEnemies} / {maxActiveEnemies}");

            if (activeEnemies < maxActiveEnemies)
            {
                Debug.Log("[Spawner] Calling SpawnEnemy...");
                SpawnEnemy();
            }
            else
            {
                Debug.LogWarning($"[Spawner] BLOCKED — {activeEnemies} < {maxActiveEnemies} is FALSE. maxActiveEnemies runtime value: {maxActiveEnemies}");
            }
        }
    }

    void SpawnEnemy()
    {
        Debug.Log($"[Spawner] SpawnEnemy entered. fallingEnemy={fallingEnemy}, terrain={terrain}");

        if (fallingEnemy == null)
        {
            Debug.LogError("[Spawner] fallingEnemy prefab is NULL — assign it in the Inspector!");
            return;
        }

        if (terrain == null)
        {
            Debug.LogError("[Spawner] terrain is NULL — assign it in the Inspector!");
            return;
        }

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(0f, spawnRadius);

        Vector3 spawnPos = transform.position + new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );

        spawnPos.y = terrain.SampleHeight(spawnPos) + spawnHeight;

        GameObject enemy = Instantiate(fallingEnemy, spawnPos, Quaternion.identity);
        activeEnemies++;

        Debug.Log($"[Spawner] Spawned enemy at {spawnPos} — activeEnemies now: {activeEnemies}");
    }

    void OnDisable()
    {
        activeEnemies = 0;
    }

    void OnApplicationQuit()
    {
        activeEnemies = 0;
    }
}