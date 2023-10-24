using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using FMODUnity;

public class WaveSystem : MonoBehaviour
{
    #region Parameters
    [SerializeField] GameObject spawnPuff;

    [Serializable]
    public struct Enemy
    {
        public string name;
        public GameObject prefab;
        public EventReference spawnSound;
        [Tooltip("[value]spawnChance in [value]totalSpawnChance")]
        public float spawnChance;
        public int unlockAtWave;
    }

    [Serializable]
    public struct Area
    {
        public string name;
        public int unlockAtWave;
        public Transform spawnpoint;
        public Animator gateAnimator;
        public TMP_Text cardinalPointOnMinimap;
    }

    [Header("Wave Debug")]
    public bool waveIsInProgress;
    [SerializeField] private int currentWaveNumber = 1;
    [SerializeField] private int enemiesKilledInCurrentWave = 0;
    [SerializeField] private List<GameObject> enemiesSpawnedInCurrentWave = new List<GameObject>();

    [Header("Spawn Chance Debug")]
    [SerializeField][Tooltip("Enemies spawn chance added together")]
    private float totalSpawnChance;
    [SerializeField][Tooltip("Random value from 0 to totalSpawnChance")]
    private float randomSpawnValue;
    [SerializeField][Tooltip("Determining which enemy to spawn")]
    private float chanceCounter;

    [Header("References")]
    [SerializeField] private TMP_Text waveTimerText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private GameObject minimap;
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
    [SerializeField] private Area[] enemySpawnAreas;
    [SerializeField] private List<Area> availableEnemySpawnAreas;
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
        defensePointInteractable.ableToInteract = false;
        waveTimerText.text = "";
    }

    #endregion

    #region Public Methods

    public void StartGame()
    {
        defensePointInteractable.ableToInteract = true;
        waveTimerText.text = gameStartMessage;
        waveTimerText.GetComponent<Animator>().Play("TextScaleIn");
        minimap.SetActive(false);
    }

    public void NewWave()
    {
        waveIsInProgress = true;
        waveText.text = "Wave " + currentWaveNumber;
        waveText.gameObject.GetComponent<Animator>().Play("TextScaleInAndOut");

        FMODAudio.Instance.betweenWavesSnapshot.Stop();
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.waveStart);

        minimap.SetActive(true);

        foreach (Area area in enemySpawnAreas)
        {
            if (currentWaveNumber == area.unlockAtWave)
            {
                area.gateAnimator.Play("DoubleGateOpen");
                FMODAudio.Instance.PlayAudio(FMODAudio.Instance.gateOpen);
                area.cardinalPointOnMinimap.color = Color.red;
                availableEnemySpawnAreas.Add(area);
                Debug.Log("Unlocked area " + area.name);
            }

        }

        foreach(Enemy enemy in enemies)
        {
            if(currentWaveNumber == enemy.unlockAtWave)
            {
                spawnableEnemies.Add(enemy);
                Debug.Log("Unlocked enemy " + enemy.name);
            }
        }

        StartCoroutine(WaveRoutine(waveDuration));
    } 

    public void KillEnemy()
    {
        enemiesKilledInCurrentWave++;
    }

    public void Defeat()
    {
        waveIsInProgress = false;
        waveTimerText.text = "It was inevitable.";
        Invoke("HideMessage", 3f);
    }

    #endregion

    #region Private Methods
    private void HideMessage()
    {
        waveTimerText.enabled = false;
    }

    private void SpawnEnemy()
    {
        if(LoseSystem.Instance.Lose) { return; }

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
                int randomArea = Random.Range(0, availableEnemySpawnAreas.Count);
                int newRandomArea = -1;
                int enemiesToSpawn = (int)Random.Range(enemiesPerSpawn / 2, enemiesPerSpawn+1);
                enemiesToSpawn = Mathf.Clamp(enemiesToSpawn, 1, (int)enemiesPerSpawnMaximum);

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    Vector3 randomPosInArea = availableEnemySpawnAreas[randomArea].spawnpoint.position + Random.insideUnitSphere * spawnRadius;
                    randomPosInArea.y = availableEnemySpawnAreas[0].spawnpoint.position.y;
                    GameObject spawnedEnemy = Instantiate(enemy.prefab, randomPosInArea, Quaternion.identity);
                    Instantiate(spawnPuff, randomPosInArea, Quaternion.identity);
                    FMODAudio.Instance.PlayAudio(enemy.spawnSound, spawnedEnemy.transform.position);
                    enemiesSpawnedInCurrentWave.Add(spawnedEnemy);
                    
                    if(i >= maxEnemiesInArea-1 && newRandomArea == -1)
                    {
                        newRandomArea = Random.Range(0, availableEnemySpawnAreas.Count);
                        while (newRandomArea == randomArea)
                            newRandomArea = Random.Range(0, availableEnemySpawnAreas.Count);

                        randomArea = newRandomArea;
                    }
                }

                Debug.Log("Spawned " + enemiesToSpawn + " enemies at " + availableEnemySpawnAreas[randomArea].name + " gate");
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
        if (LoseSystem.Instance.Lose) { return; }

        waveIsInProgress = false;
        waveText.text = "Wave " + currentWaveNumber + " Completed";
        waveText.gameObject.GetComponent<Animator>().Play("TextScaleInAndOut");
        currentWaveNumber++;

        FMODAudio.Instance.betweenWavesSnapshot.Play();
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.waveEnd);

        minimap.SetActive(false);

        /*foreach (GameObject enemy in enemiesSpawnedInCurrentWave)
        {
            if (enemy != null)
                enemy.GetComponent<Health>().Death();
        }*/

        enemiesSpawnedInCurrentWave = new List<GameObject>();
        enemiesKilledInCurrentWave = 0;
        waveTimerText.text = waveEndMessage;
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
        InvokeRepeating(nameof(SpawnEnemy), 4.0f, spawnRate);

        float timeElapsed = waveDuration;
        while(timeElapsed > 0)
        {
            timeElapsed -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeElapsed / 60F);
            int seconds = Mathf.FloorToInt(timeElapsed - minutes * 60);
            waveTimerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

            yield return null;
        }
        CancelInvoke();

        while (enemiesKilledInCurrentWave < enemiesSpawnedInCurrentWave.Count)
        {
            int enemiesLeftToKill = enemiesSpawnedInCurrentWave.Count - enemiesKilledInCurrentWave;
            string message = enemiesLeftToKill == 1 ? "enemy left to kill" : "enemies left to kill";
            waveTimerText.text = enemiesLeftToKill + " " + message;
            yield return null;
        } 
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
