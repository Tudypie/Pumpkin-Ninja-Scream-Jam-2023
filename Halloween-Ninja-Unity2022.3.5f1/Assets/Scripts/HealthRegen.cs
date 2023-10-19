using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthRegen : MonoBehaviour
{
    Health health;
    [Tooltip("Number of HP gained per minute")]
    [SerializeField] int healthRegen = 0;
    [Tooltip("How often natural healing ticks in seconds.  Set to 0 for OnUpdate")]
    [SerializeField] float regenTickRate = 0;
    float healingReserve;
    float nextHealTime;
    

    // Start is called before the first frame update
    void Awake()
    {
        nextHealTime = Time.time + regenTickRate;
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        healingReserve += Time.deltaTime * healthRegen/60;
        if (Time.time > nextHealTime)
        {
            health.Heal(healingReserve);
            healingReserve = 0;
            nextHealTime = Time.time + regenTickRate;
        }
    }

}
