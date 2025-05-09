using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject enemyPrefab;
    public float timeBetweenWaves = 5f;
    public int maxActiveEnemies = 5;

    //TODO: CHOOSE SPAWN AREA DEPENDING ON NAVMESH
    [Header("Spawn Area")]
    public Vector2 spawnMin;
    public Vector2 spawnMax;

    private int currentWaveIndex = 0;
    private int currentActiveEnemies = 0;
    private bool spawning = false;

    void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            yield return StartCoroutine(HandleWaveSpawning());
            currentWaveIndex++;
        }
    }

    private IEnumerator HandleWaveSpawning()
    {
        spawning = true;
        
        int numberOfEnemies = 5 + currentWaveIndex * 2; 
        float spawnInterval = 1.5f - currentWaveIndex * 0.1f; 
        
        spawnInterval = Mathf.Max(spawnInterval, 0.1f);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            while (currentActiveEnemies >= maxActiveEnemies)
                yield return null;

            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        spawning = false;
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        currentActiveEnemies++;

        var enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.HealthController.OnDeath += () =>
            {
                currentActiveEnemies--;
            };
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnMin.x, spawnMax.x);
        float z = Random.Range(spawnMin.y, spawnMax.y); 
        float y = 100f;
        return new Vector3(x, y, z);
    }
}
