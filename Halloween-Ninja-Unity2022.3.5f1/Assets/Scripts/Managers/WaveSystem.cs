using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSystem : MonoBehaviour
{
    #region Parameters

    [Serializable]
    public struct Enemy
    {
        public string name;
        public GameObject prefab;
        [Tooltip("[value]spawnChance in [value]totalSpawnChance")]
        public float spawnChance;
    }

    [Header("Wave Debug")]
    [SerializeField] private bool waveIsInProgress;
    [SerializeField] private int currentWaveNumber = 0;
    [SerializeField] private int enemiesKilledInCurrentWave = 0;

    [Header("Spawn Debug")]
    [SerializeField] [Tooltip("Enemies spawn chance added together")]
    private float totalSpawnChance;
    [SerializeField] [Tooltip("Random value from 0 to totalSpawnChance")] 
    private float randomSpawnValue;
    [SerializeField] [Tooltip("Determining which enemy to spawn")] 
    private float chanceCounter;

    [Header("Wave Settings")]
    [SerializeField] private Vector2 waveDurationMinMax;
    [SerializeField] private Vector2 enemiesToKillMinMax;

    [Header("Spawn Rate")]
    [SerializeField] private float spawnRate = 5.0f;
    [SerializeField] private float spawnRateMinimum = 0.5f;
    [SerializeField] private float spawnRateDecrease = 0.75f;
    [SerializeField] private int everyAmountOfWaves = 4;

    [Header("Enemies")]
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private List<Enemy> spawnableEnemies;

    [Header("Areas")]
    [SerializeField] private Transform[] enemySpawnAreas;
    [SerializeField] private float spawnRadius = 5.0f;

    public static WaveSystem Instance;

    #endregion

    #region Default Methods

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }

        foreach (Enemy enemy in enemies)
            spawnableEnemies.Add(enemy);
    }

    #endregion

    #region Public Methods

    public void NewWave()
    {
        float randomWave = Random.Range(0, 2);
        if(randomWave == 0)
        {
            NewWaveWithDuration((int)Random.Range(waveDurationMinMax.x, waveDurationMinMax.y));
        }
        else if(randomWave == 1)
        {
            NewWaveWithEnemiesToKill((int)Random.Range(enemiesToKillMinMax.x, enemiesToKillMinMax.y));
        }
    }

    public void NewWaveWithDuration(float duration) => StartCoroutine(WaveRoutine(waveDuration: duration));

    public void NewWaveWithEnemiesToKill(int enemiesToKill) => StartCoroutine(WaveRoutine(enemiesToKill: enemiesToKill));

    public void NewSpawnableEnemy(int enemyIndex) => spawnableEnemies.Add(enemies[enemyIndex]);

    public void KillEnemy() => enemiesKilledInCurrentWave++;

    #endregion

    #region Private Methods

    private void SpawnEnemy()
    {
        if (spawnableEnemies.Count == 0)
        {
            Debug.LogError("No spawnable enemies. Add enemies to spawnableEnemies list.");
            return;
        }

        totalSpawnChance = 0f;
        foreach (Enemy enemy in spawnableEnemies) 
            totalSpawnChance += enemy.spawnChance;

        randomSpawnValue = Random.Range(0f, totalSpawnChance);

        chanceCounter = 0f;
        foreach (Enemy enemy in spawnableEnemies)
        {
            chanceCounter += enemy.spawnChance;
            if (randomSpawnValue <= chanceCounter)
            {
                Vector3 randomPosInArea = enemySpawnAreas[Random.Range(0, enemySpawnAreas.Length)].position + Random.insideUnitSphere * spawnRadius;
                randomPosInArea.y = enemySpawnAreas[0].position.y;
                Instantiate(enemy.prefab, randomPosInArea, Quaternion.identity);
                break;
            }
        }

        /*
        * Example:
        * enemy1Chance = 1, enemy2Chance = 2, enemy3Chance = 3
        * totalSpawnChance = 6
        * enemy1Chance -> 1 in 6, enemy2Chance -> 2 in 6, enemy3Chance -> 3 in 6
        * randomSpawnValue = RandomRange[0, 6] -> ex: randomSpawnValue = 3
        * chanceCounter = 1
        * chanceCounter = 1 + 2 = 3 -> randomSpawnValue <= chanceCounter -> 3 <= 3 -> spawn enemy2
        * chanceCounter = 1 + 2 + 3 = 6
        */
    }


    private void EndWave()
    {
        waveIsInProgress = false;
        currentWaveNumber++;

        if(currentWaveNumber % everyAmountOfWaves == 0)
        {
            spawnRate -= spawnRateDecrease;
            spawnRate = Mathf.Max(spawnRate, spawnRateMinimum);
        }
    }

    private IEnumerator WaveRoutine(float waveDuration)
    {
        waveIsInProgress = true;
        InvokeRepeating(nameof(SpawnEnemy), spawnRate, spawnRate);
        float timeElapsed = 0;
        while(timeElapsed < waveDuration)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        CancelInvoke();
        EndWave();
    }

    private IEnumerator WaveRoutine(int enemiesToKill)
    {
        waveIsInProgress = true;
        enemiesKilledInCurrentWave = 0;
        while (enemiesKilledInCurrentWave < enemiesToKill)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
        EndWave();
    }

    #endregion
}
