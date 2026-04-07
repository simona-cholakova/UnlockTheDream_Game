using UnityEngine;

public class FinalKeyProgress : MonoBehaviour
{
    public static FinalKeyProgress instance;

    [Header("Requirements")]
    public int stormcallerKills = 0;
    public int frostwalkerBlocks = 0;

    public int requiredKills = 5;
    public int requiredBlocks = 4;

    [Header("Spawn")]
    public GameObject key3Prefab;
    public Transform player;
    public float spawnDistance = 3f;
    public Transform playerCamera;

    private bool spawned = false;

    void Awake() => instance = this;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnKey();
        }
    }

    public void AddStormcallerKill()
    {
        stormcallerKills++;
        Debug.Log("Stormcaller kills: " + stormcallerKills);
        CheckSpawn();
    }

    public void AddFrostwalkerBlock()
    {
        frostwalkerBlocks++;
        Debug.Log("Frostwalker blocks: " + frostwalkerBlocks);
        CheckSpawn();
    }


    void CheckSpawn()
    {
        Debug.Log($"CHECK → kills: {stormcallerKills}/{requiredKills}, blocks: {frostwalkerBlocks}/{requiredBlocks}");

        if (spawned)
        {
            Debug.Log("Already spawned, skipping");
            return;
        }

        if (stormcallerKills >= requiredKills &&
            frostwalkerBlocks >= requiredBlocks)
        {
            Debug.Log("🎉 CONDITIONS MET → spawning");
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