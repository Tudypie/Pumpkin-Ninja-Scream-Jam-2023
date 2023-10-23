using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossPumpkinSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnerProjectile;
    [SerializeField] Transform[] enemyPrefabs;
    Transform player;

    [SerializeField] float spawnFrequency = 2f;
    float pumpkinSpawnRange = 4;

    float nextSpawnTime;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime) SpawnPumpkin();
    }

    void SpawnPumpkin()
    {
        nextSpawnTime = Time.time + spawnFrequency;

        #region GetRandomSpawnLocation

        Vector3 defaultSpawnLocation;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas);
        defaultSpawnLocation = hit.position;

        Vector3 offsetLocation = new Vector3(Random.Range(-pumpkinSpawnRange, pumpkinSpawnRange), 0, Random.Range(-pumpkinSpawnRange, pumpkinSpawnRange));
        NavMesh.SamplePosition(transform.position + offsetLocation, out hit, 100, NavMesh.AllAreas);

        Vector3 spawnLocation;
        if (hit.position.x < 11.5 && hit.position.x > -11.5 && hit.position.z < 11.5 && hit.position.x > -11.5)
            spawnLocation = hit.position;
        else
        {
            spawnLocation = defaultSpawnLocation;
        }

        #endregion

        #region GetRandomPumpkin
        Transform pumpkinToSpawn = enemyPrefabs[Random.Range(0,enemyPrefabs.Length)];
        #endregion

        #region SpawnPumpkin
        Transform pumpkinSoul = Instantiate(spawnerProjectile, transform.position, Quaternion.identity);

        pumpkinSoul.GetComponent<SpawnerProjectile>().Setup(spawnLocation,pumpkinToSpawn,player);
        #endregion
    }

    void SpawnPumpkinAtPosition(Vector3 position, Transform prefab)
    {
        Transform spawnedPumpkin = Instantiate(prefab, position, Quaternion.identity);
        spawnedPumpkin.GetComponent<EnemyBehaviour>().Setup(player);
        
    }

}
