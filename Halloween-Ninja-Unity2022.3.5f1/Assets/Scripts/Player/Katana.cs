using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Animator anim;
    [SerializeField] TriggerRelay trigger;
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] LayerMask dashMask;
    [SerializeField] KeyCode attackKey;

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

        if (hitGameGbject.CompareTag("DefensePoint")) return;

        if (hitThisAttack.Contains(hitGameGbject)) return;
        hitThisAttack.Add(hitGameGbject);

        float multiplier = 1;
        ShurikenTag shurikenTag = e.other.gameObject.GetComponentInParent<ShurikenTag>();
        if (shurikenTag)
        {
            if (shurikenTag.tagged)
            {
                multiplier = 3f;
            }
        }

        Health hitHealth = hitGameGbject.GetComponentInParent<Health>();
        if (hitHealth == null) return;

        hitHealth.TakeDamage(damage * multiplier);
        
    }

    void Update()
    {
        if(Input.GetKeyDown(attackKey) && Time.time > nextAttackTime){
            KatanaStrike();
        }
    }

    void KatanaStrike()
    {
        bool isTagged = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, dashSpeed*0.75f, dashMask))
        {
            isTagged = hit.collider.transform.GetComponentInParent<ShurikenTag>().tagged;
        }
        if(isTagged)
        StartCoroutine(IKatanaDashStrike());
        else
        StartCoroutine(IKatanaStrike());

    }

    IEnumerator IKatanaStrike()
    {
        nextAttackTime = Time.time + attackCooldown;
        anim.SetTrigger("Melee1");
        
        yield return new WaitForSeconds(attackBeginTime);
        canDamage = true;
        yield return new WaitForSeconds(attackEndTime - attackBeginTime);
        canDamage = false;
        hitThisAttack = new List<GameObject>();
    }

    IEnumerator IKatanaDashStrike()
    {
        nextAttackTime = Time.time + attackCooldown;
        anim.SetTrigger("Melee1");

        
        Vector3 targetPosition = transform.position + transform.forward * 8;
        Vector3 dashVelocity = transform.forward * dashSpeed;
        CharacterController playerController = transform.parent.GetComponentInParent<CharacterController>();


        float timer = 0;
        while (timer < attackEndTime)
        {

            playerController.Move(dashVelocity * Time.deltaTime);

            if (timer > attackBeginTime)
            {
                canDamage = true;
            }
            if (timer > attackEndTime - attackBeginTime)
            {
                canDamage = false;
            }
            
            timer += Time.deltaTime;
            yield return 0;
        }

        hitThisAttack = new List<GameObject>();
    }

}
