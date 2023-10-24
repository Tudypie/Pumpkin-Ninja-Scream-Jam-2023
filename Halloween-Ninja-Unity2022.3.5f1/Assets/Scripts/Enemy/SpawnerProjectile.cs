using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

public class SpawnerProjectile : MonoBehaviour
{
    [SerializeField] Transform spawnExplosion;
    Transform enemyTarget;

    Transform prefabToSpawn;
    [SerializeField] TrailRenderer trailRenderer;

    [SerializeField] private AnimationCurve arcYAnimationCurve;

    [SerializeField] float moveSpeed = 15f;

    private Vector3 targetPosition;

    private float totalDistance;
    private Vector3 positionXZ;

    private void Update()
    {

        Vector3 moveDir = (targetPosition - positionXZ).normalized;


        positionXZ += moveDir * moveSpeed * Time.deltaTime;


        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;


        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;

        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetPosition = 0.2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetPosition)
        {

            if (trailRenderer) trailRenderer.transform.parent = null;
            Instantiate(spawnExplosion, targetPosition + Vector3.up * 0.3f, Quaternion.identity);
            Transform objectSpawned = Instantiate(prefabToSpawn, targetPosition + Vector3.up * 0.3f, Quaternion.identity);
            objectSpawned.GetComponent<EnemyBehaviour>().Setup(enemyTarget);
            objectSpawned.GetComponent<DamageOnCollision>().damagePlayer = true;

            Destroy(gameObject);
        }


    }

    public void Setup(Vector3 targetPosition, Transform prefabToSpawn, Transform enemyTarget, EventReference spawnSound)
    {
        this.prefabToSpawn = prefabToSpawn;

        positionXZ = transform.position;
        positionXZ.y = 0;

        totalDistance = Vector3.Distance(positionXZ, targetPosition);

        this.enemyTarget = enemyTarget;
        FMODAudio.Instance.PlayAudio(spawnSound, transform.position);
    }


}

