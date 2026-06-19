using UnityEngine;

public class FinalKeyProgress : MonoBehaviour
{
    public static FinalKeyProgress instance;

    [Header("Requirements")]
    public int stormcallerKills = 0;
    public int requiredKills = 5;

    [Header("Spawn")]
    public GameObject key3Prefab;
    public Transform player;
    public float spawnDistance = 3f;
    public Transform playerCamera;

    private bool spawned = false;

    void Awake() => instance = this;

    public void AddStormcallerKill()
    {
        stormcallerKills++;

        Debug.Log($"CHECK → kills: {stormcallerKills}/{requiredKills}");

        if (!spawned && stormcallerKills >= requiredKills)
        {
            SpawnKey();
            spawned = true;
        }
    }

    void SpawnKey()
    {
        Vector3 forwardDir = playerCamera.forward;
        forwardDir.y = 0f;
        forwardDir.Normalize();

        Vector3 spawnPos = player.position
                         + forwardDir * (spawnDistance + 3f)
                         + Vector3.up * 0.5f;

        GameObject key = Instantiate(key3Prefab, spawnPos, Quaternion.identity);

        key.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        key.SetActive(true);

        Debug.Log("KEY 3 SPAWNED");
    }
}