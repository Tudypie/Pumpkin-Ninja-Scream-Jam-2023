using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

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
    [SerializeField] private List<GameObject> enemiesSpawnedInCurrentWave = new List<GameObject>();

    [Header("Spawn Chance Debug")]
    [SerializeField] [Tooltip("Enemies spawn chance added together")]
    private float totalSpawnChance;
    [SerializeField] [Tooltip("Random value from 0 to totalSpawnChance")] 
    private float randomSpawnValue;
    [SerializeField] [Tooltip("Determining which enemy to spawn")] 
    private float chanceCounter;

    [Header("References")]
    [SerializeField] private TMP_Text waveStatusText;
    [SerializeField] private DefensePoint defensePoint;
    private Interactable defensePointInteractable;

    [Header("Wave Settings")]
    [SerializeField] private float enemiesPerSpawn = 1;
    [SerializeField] private float enemiesPerSpawnMaximum = 6;
    [SerializeField] private float enemiesPerSpawnIncrease = 0.5f;
    [SerializeField] private float maxEnemiesInArea = 4;
    [Space]
    [SerializeField] private float spawnRate = 5.0f;
    [SerializeField] private float spawnRateMinimum = 0.5f;
    [SerializeField] private float spawnRateDecrease = 0.75f;
    [Space]
    [SerializeField] private float waveDuration;
    [SerializeField] private float waveDurationMaximum = 120.0f;
    [SerializeField] private float waveDurationIncrease = 10.0f;
    [Space]
    [SerializeField] private int difficultyIncreaseEveryWaves = 1;

    [Header("Messages")]
    [SerializeField] private string gameStartMessage;
    [SerializeField] private string gameEndMessage;
    [SerializeField] private string waveEndMessage;

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

        defensePointInteractable = defensePoint.gameObject.GetComponent<Interactable>();

        foreach (Enemy enemy in enemies)
            spawnableEnemies.Add(enemy);

        waveStatusText.text = gameStartMessage;
    }

    #endregion

    #region Public Methods

    public void NewWave() => StartCoroutine(WaveRoutine(waveDuration));

    public void NewSpawnableEnemy(int enemyIndex) => spawnableEnemies.Add(enemies[enemyIndex]);

    public void KillEnemy() => enemiesKilledInCurrentWave++;

    public void EndWaveSystem()
    {
        waveIsInProgress = false;
        waveStatusText.text = "Game Over.";
    }

    #endregion

    #region Private Methods

    private void SpawnEnemy()
    {
        if(LoseGame.Instance.Lose) { return; }

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
                int randomArea = Random.Range(0, enemySpawnAreas.Length);
                int newRandomArea = -1;
                int enemiesToSpawn = (int)Random.Range(enemiesPerSpawn / 2, enemiesPerSpawn+1);
                enemiesToSpawn = Mathf.Clamp(enemiesToSpawn, 1, (int)enemiesPerSpawnMaximum);
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    Vector3 randomPosInArea = enemySpawnAreas[randomArea].position + Random.insideUnitSphere * spawnRadius;
                    randomPosInArea.y = enemySpawnAreas[0].position.y;
                    GameObject spawnedEnemy = Instantiate(enemy.prefab, randomPosInArea, Quaternion.identity);
                    enemiesSpawnedInCurrentWave.Add(spawnedEnemy);
                    
                    if(i >= maxEnemiesInArea-1 && newRandomArea == -1)
                    {
                        newRandomArea = Random.Range(0, enemySpawnAreas.Length);
                        while (newRandomArea == randomArea)
                            newRandomArea = Random.Range(0, enemySpawnAreas.Length);
                        randomArea = newRandomArea;
                    }

                }
                Debug.Log("Spawned " + enemiesToSpawn + " enemies at " + enemySpawnAreas[randomArea].name + " gate");
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
        if (LoseGame.Instance.Lose) { return; }

        waveIsInProgress = false;
        currentWaveNumber++;

        foreach (GameObject enemy in enemiesSpawnedInCurrentWave)
        {
            if (enemy != null)
                enemy.GetComponent<Health>().Death();
        }

        enemiesSpawnedInCurrentWave = new List<GameObject>();
        enemiesKilledInCurrentWave = 0;
        waveStatusText.text = waveEndMessage;
        defensePointInteractable.ableToInteract = true;

        if(currentWaveNumber % difficultyIncreaseEveryWaves == 0)
        {
            enemiesPerSpawn += enemiesPerSpawnIncrease;
            enemiesPerSpawn = Mathf.Min(enemiesPerSpawn, enemiesPerSpawnMaximum);

            spawnRate -= spawnRateDecrease;
            spawnRate = Mathf.Max(spawnRate, spawnRateMinimum);

            waveDuration += waveDurationIncrease;
            waveDuration = Mathf.Min(waveDuration, waveDurationMaximum);
        }
    }

    private IEnumerator WaveRoutine(float waveDuration)
    {
        waveIsInProgress = true;
        InvokeRepeating(nameof(SpawnEnemy), 0.0f, spawnRate);

        float timeElapsed = waveDuration;
        while(timeElapsed > 0)
        {
            timeElapsed -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeElapsed / 60F);
            int seconds = Mathf.FloorToInt(timeElapsed - minutes * 60);
            waveStatusText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

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
