using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DefensePoint"))
        {
            other.gameObject.TryGetComponent(out Health defensePointHealth);
            this.gameObject.TryGetComponent(out Health enemyHealth);

            defensePointHealth.TakeDamage(damage);
            enemyHealth.Death();
        }
    }
}
