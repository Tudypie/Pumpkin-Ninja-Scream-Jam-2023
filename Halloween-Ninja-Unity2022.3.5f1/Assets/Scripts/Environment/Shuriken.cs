using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    
    [SerializeField] float damage = 10f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")
            || other.gameObject.CompareTag("DefensePoint")
            || other.gameObject.CompareTag("DamageBox")) 
        { 
            return; 
        }

        Health health = other.gameObject.GetComponentInParent<Health>();
        if (health)
            health.TakeDamage(damage);

        float multiplier = 1;
        ShurikenTag shurikenTag = other.gameObject.GetComponentInParent<ShurikenTag>();
        if (shurikenTag)
            shurikenTag.tagged = true;

        GetComponentInChildren<Animator>().speed = 0;
        Destroy(gameObject, 3f);
        enabled = false;
    }



}
