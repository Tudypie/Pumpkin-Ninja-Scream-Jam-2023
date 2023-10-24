using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;
    public bool damagePlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        Transform hitParent = other.transform.parent;
        Transform hit = hitParent == null ? other.transform : hitParent;
        if (hit.tag != "Player" && hit.tag != "DefensePoint") return;
        if (hit.tag == "Player" && !damagePlayer) return;


        hit.gameObject.TryGetComponent(out Health targetHealth);
        targetHealth.TakeDamage(damage);

        this.gameObject.TryGetComponent(out Health thisUnitHealth);
        thisUnitHealth.Death();
    }

}
