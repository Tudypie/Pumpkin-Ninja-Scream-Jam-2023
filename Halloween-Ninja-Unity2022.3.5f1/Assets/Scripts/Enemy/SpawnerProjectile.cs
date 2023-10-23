using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerProjectile : MonoBehaviour
{

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
            Transform objectSpawned = Instantiate(prefabToSpawn, targetPosition + Vector3.up * 0.3f, Quaternion.identity);
            objectSpawned.GetComponent<EnemyBehaviour>().Setup(enemyTarget);

            Destroy(gameObject);
        }


    }

    public void Setup(Vector3 targetPosition, Transform prefabToSpawn, Transform enemyTarget)
    {
        this.prefabToSpawn = prefabToSpawn;

        positionXZ = transform.position;
        positionXZ.y = 0;

        totalDistance = Vector3.Distance(positionXZ, targetPosition);

        this.enemyTarget = enemyTarget;
    }


}

