using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using FMODUnity;

public class BossPumpkinSpawner : MonoBehaviour
{
    public EventReference[] spawnSounds;
    [SerializeField] Transform spawnerProjectile;

    [SerializeField] Transform[] enemyPrefabs;
    Transform player;

    [SerializeField] float spawnFrequency = 2.5f;
    [SerializeField] float burstSpawnRate = 1.5f;

    Health health;

    float pumpkinSpawnRange = 4;


    float nextSpawnTime;

    float startSpawningTime;

    int nextStageHealth = 90;

    float nextLaughTime;

    public EventReference laugh;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        startSpawningTime = Time.time + 3f;
        health = GetComponent<Health>();
        health.OnTakeDamage += TakeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime) SpawnPumpkin();
        if (Time.time > nextLaughTime && transform.position.y > 10f)
        {
            nextLaughTime = Time.time + 5f;
            FMODAudio.Instance.PlayAudio(laugh, player.position);
        }
    }

    void SpawnPumpkin()
    {
        
        nextSpawnTime = (transform.position.y > 10f)? Time.time + burstSpawnRate : Time.time + spawnFrequency;

        #region GetRandomSpawnLocation

        Vector3 defaultSpawnLocation;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas);
        defaultSpawnLocation = hit.position;

        Vector3 randomSpawnPosition = new Vector3(UnityEngine.Random.Range(-11.5f, 11.5f), 0, UnityEngine.Random.Range(-11.5f, 11.5f));
        NavMesh.SamplePosition(randomSpawnPosition, out hit, 100, NavMesh.AllAreas);

        Vector3 spawnLocation;
        if (transform.position.y > 10f)
            spawnLocation = hit.position;
        else
        {
            spawnLocation = defaultSpawnLocation;
        }

        #endregion

        #region GetRandomPumpkin
        int rng = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        Transform pumpkinToSpawn = enemyPrefabs[rng];
        EventReference spawnSound = spawnSounds[rng];
        #endregion

        #region SpawnPumpkin
        Transform pumpkinSoul = Instantiate(spawnerProjectile, transform.position, Quaternion.identity);

        pumpkinSoul.GetComponent<SpawnerProjectile>().Setup(spawnLocation,pumpkinToSpawn,player, spawnSound);
        #endregion

    }


    void TakeDamage(object sender, EventArgs e)
    {
        if (health.currenthealth < nextStageHealth)
        {
            nextStageHealth -= 10;
            burstSpawnRate -= 0.1f;
            spawnFrequency -= 0.1f;
        }
    }

}
