using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Animator anim;
    [SerializeField] TriggerRelay trigger;
    List<GameObject> hitThisAttack = new List<GameObject>();
    float nextAttackTime = 0;
    float attackCooldown = 0.5f;
    float attackBeginTime = 0.05f;
    float attackEndTime = 0.3f;
    bool canDamage = false;

    private void Start()
    {
        trigger.OnTriggerEnterEvent += TriggerRelay_OnTriggerEnter;
        trigger.enabled = false;
    }

    private void TriggerRelay_OnTriggerEnter(object sender, TriggerRelay.CollisionEvent e)
    {
        if (!canDamage) return;
        GameObject hitGameGbject = e.other.gameObject;

        if (hitThisAttack.Contains(hitGameGbject)) return;
        hitThisAttack.Add(hitGameGbject);


        Health hitHealth = hitGameGbject.GetComponentInParent<Health>();
        if (hitHealth == null) return;

        hitHealth.TakeDamage(damage);
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && Time.time > nextAttackTime){
            KatanaStrike();
        }
    }

    void KatanaStrike()
    {
        nextAttackTime = Time.time + attackCooldown;
        anim.SetTrigger("Melee1");
        StartCoroutine(IKatanaStrike());
    }

    IEnumerator IKatanaStrike()
    {
        
        yield return new WaitForSeconds(attackBeginTime);
        canDamage = true;
        yield return new WaitForSeconds(attackEndTime - attackBeginTime);
        canDamage = false;
        hitThisAttack = new List<GameObject>();
    }
}
