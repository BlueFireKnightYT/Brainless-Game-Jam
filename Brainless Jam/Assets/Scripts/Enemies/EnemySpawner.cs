using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public List<GameObject> allEnemies = new List<GameObject>();
    public int currentWaveCount;

    [System.Serializable]
    public class EnemyWave
    {
        public int waveCount;
        public float delayUntilSpawn;
        public int enemyAmount;
        public GameObject[] enemyToSpawn;
        public float density;
        public bool randomSpawnLocation;
        public int spawnLocation;
    }
    [Header("Enemy Waves")]
    public EnemyWave[] waves;

    public bool waveActive = true;

    public float spawnCooldown = 1;


    void Start()
    {
        if (WaveData.waveData != null && WaveData.waveData.waves != null)
        {
            waves = WaveData.waveData.waves;
        }
            StartCoroutine(WaveAdvancer());
    }

    void EndWave()
    {
        currentWaveCount++;
        if (currentWaveCount >= 0 && currentWaveCount < waves.Length)
        {
            StartCoroutine(WaveAdvancer());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            WaveData.finished = true;
            WaveData.amountFinished++;
        }
    }

    private void Update()
    {
        allEnemies.RemoveAll(enemy => enemy == null);
        if (allEnemies.Count <= 0 && !waveActive)
        {
            waveActive = true;
            EndWave();
        }
    }

    IEnumerator WaveAdvancer()
    {
        if (waves[currentWaveCount] != null)
        {
            EnemyWave currentWave = waves[currentWaveCount];
            yield return new WaitForSeconds(currentWave.delayUntilSpawn);
            int chosenSpawnPoint = 0;
            for (int i = 0; i < currentWave.enemyAmount; i++)
            {
                yield return new WaitForSeconds(currentWave.density);
                if (currentWave.randomSpawnLocation)
                {
                    chosenSpawnPoint = Random.Range(0, spawnPoints.Length);
                }
                else
                {
                    chosenSpawnPoint = currentWave.spawnLocation;
                }
                GameObject spawnedEnemy = Instantiate(currentWave.enemyToSpawn[i], spawnPoints[chosenSpawnPoint].position, Quaternion.identity);
                spawnedEnemy.GetComponent<EnemyPathMover>().cEP = spawnPoints[chosenSpawnPoint].GetComponent<CreateEnemyPath>();
                allEnemies.Add(spawnedEnemy);
                allEnemies.RemoveAll(item => item == null);
            }
            waveActive = false;
        }  
    }
}
