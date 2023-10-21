using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    
    float damage = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { return; }

        Destroy(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) { return; }

        GetComponentInChildren<Animator>().speed = 0;

        Health health = other.gameObject.GetComponentInParent<Health>();
        if (health)
        {
            health.TakeDamage(damage);
        }

        enabled = false;

    }



}
