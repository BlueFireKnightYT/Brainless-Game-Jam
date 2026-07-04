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

    float spawnCooldown = 1;


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
    }

    IEnumerator spawnTimer()
    {
        while (canSpawn)
        { 
            yield return new WaitForSeconds(spawnCooldown);
            int chosenSpawnPoint = Random.Range(0, spawnPoints.Length);
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoints[chosenSpawnPoint].position, Quaternion.identity);
            allEnemies.Add(spawnedEnemy);
            allEnemies.RemoveAll(item => item == null);
        }
    }
}
