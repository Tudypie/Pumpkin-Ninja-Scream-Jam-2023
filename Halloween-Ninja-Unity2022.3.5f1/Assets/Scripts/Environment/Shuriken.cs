using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    
    [SerializeField] float damage = 10f;
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

        float multiplier = 1;
        ShurikenTag shurikenTag = other.gameObject.GetComponentInParent<ShurikenTag>();
        if (shurikenTag)
        {
            shurikenTag.tagged = true;
        }

        enabled = false;
        GetComponent<Collider>().enabled = false;

    }



}
