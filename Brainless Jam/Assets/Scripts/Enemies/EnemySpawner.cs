using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public List<GameObject> allEnemies = new List<GameObject>();

    public bool canSpawn = false;
    bool hasStartedSpawning = false;

    public float spawnCooldown = 1;
    int totalEnemiesSpawned;


    void Update()
    {
        if (canSpawn && !hasStartedSpawning)
        {
            StartCoroutine(spawnTimer());
            hasStartedSpawning = true;
        }
        else if (!canSpawn && hasStartedSpawning)
        {
            StopCoroutine(spawnTimer());
            hasStartedSpawning = false;
        }
        if(totalEnemiesSpawned >= 25)
        {
            spawnCooldown /= 1.2f;
            totalEnemiesSpawned = 0;
        }
    }

    IEnumerator spawnTimer()
    {
        while (canSpawn)
        { 
            yield return new WaitForSeconds(spawnCooldown);
            int chosenSpawnPoint = Random.Range(0, spawnPoints.Length);
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoints[chosenSpawnPoint].position, Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyPathMover>().cEP = spawnPoints[chosenSpawnPoint].GetComponent<CreateEnemyPath>();
            allEnemies.Add(spawnedEnemy);
            allEnemies.RemoveAll(item => item == null);
            totalEnemiesSpawned++;
        }
    }
}
