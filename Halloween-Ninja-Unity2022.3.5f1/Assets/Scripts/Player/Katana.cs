using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Animator anim;
    [SerializeField] TriggerRelay slashTrigger;
    [SerializeField] TriggerRelay dashSlashTrigger;
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] LayerMask dashMask;
    [SerializeField] KeyCode attackKey;

    [SerializeField] GameObject DashUI;

    List<GameObject> hitThisAttack = new List<GameObject>();

    //Slow Motion
    [Header("Slow Motion")]
    [SerializeField] bool useSlowMotioEffect = true;
    [SerializeField] float slowMotionTimeScale = 0.5f;
    [SerializeField] float slowMotionDuration = 1.5f;
    float lastTime, elapsed;
    bool slowDownTime = false;


    float nextAttackTime = 0;
    float attackCooldown = 0.5f;
    float attackBeginTime = 0f;
    float attackEndTime = 0.3f;
    bool canSlashDamage = false;
    bool canDashSlashDamage = false;

    private void Start()
    {
        slashTrigger.OnTriggerEnterEvent += SlashTriggerRelay_OnTriggerEnter;
        slashTrigger.enabled = false;

        dashSlashTrigger.OnTriggerEnterEvent += DashSlashTriggerRelay_OnTriggerEnter;
        dashSlashTrigger.enabled = false;
    }

    private void SlashTriggerRelay_OnTriggerEnter(object sender, TriggerRelay.CollisionEvent e)
    {

        if (!canSlashDamage) return;
        GameObject hitGameGbject = e.other.gameObject;

        if (hitGameGbject.CompareTag("DefensePoint")) return;

        if (hitThisAttack.Contains(hitGameGbject)) return;
        hitThisAttack.Add(hitGameGbject);

        Health hitHealth = hitGameGbject.GetComponentInParent<Health>();
        if (hitHealth == null) return;

        hitHealth.TakeDamage(damage);
        //FMODAudio.Instance.PlayAudio(FMODAudio.Instance.katanaSlash, transform.position);
        Debug.Log("Hit Normal Slash");

    }

    private void DashSlashTriggerRelay_OnTriggerEnter(object sender, TriggerRelay.CollisionEvent e)
    {
        if (!canDashSlashDamage) return;
        GameObject hitGameGbject = e.other.gameObject;

        if (hitGameGbject.CompareTag("DefensePoint")) return;

        if (hitThisAttack.Contains(hitGameGbject)) return;
        hitThisAttack.Add(hitGameGbject);

        Health hitHealth = hitGameGbject.GetComponentInParent<Health>();
        if (hitHealth == null) return;

        hitHealth.TakeDamage(damage * 3);
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.katanaSlash, transform.position);
        Debug.Log("Hit Dash Slash");

    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && Time.time > nextAttackTime)
        {
            KatanaStrike();
        }

        bool isTagged = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, dashSpeed * 0.75f, dashMask))
        {
            isTagged = hit.collider.transform.GetComponentInParent<ShurikenTag>().tagged;
        }
        DashUI.SetActive(isTagged);
    }

    void FixedUpdate()
    {
        if (!slowDownTime || !useSlowMotioEffect) { return; }

        if (lastTime == 0)
        {
            lastTime = Time.realtimeSinceStartup;
        }
        else
        {
            elapsed += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;
            Time.timeScale = slowMotionTimeScale;
        }

        if (elapsed >= slowMotionDuration)
        {
            Time.timeScale = 1f;
            slowDownTime = false;
        }
    }

    void SlowMotion()
    {
        slowDownTime = true;
        lastTime = 0;
        elapsed = 0;
    }

    void KatanaStrike()
    {
        bool isTagged = false;
        hitThisAttack = new List<GameObject>();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, dashSpeed*5f, dashMask))
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
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.katanaAttack, transform.position);

        yield return new WaitForSeconds(attackBeginTime);
        canSlashDamage = true;
        yield return new WaitForSeconds(attackEndTime - attackBeginTime);
        canSlashDamage = false;
        hitThisAttack = new List<GameObject>();
    }

    IEnumerator IKatanaDashStrike()
    {
        nextAttackTime = Time.time + attackCooldown;
        anim.SetTrigger("Melee1");
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.katanaDash, transform.position);
        SlowMotion();

        Vector3 targetPosition = transform.position + transform.forward * 8;
        Vector3 dashVelocity = transform.forward * dashSpeed;
        CharacterController playerController = transform.parent.GetComponentInParent<CharacterController>();


        float timer = 0;
        while (timer < attackEndTime)
        {

            playerController.Move(dashVelocity * Time.deltaTime);

            canDashSlashDamage = true;

            
            timer += Time.deltaTime;
            yield return 0;
        }
        canDashSlashDamage = false;
        hitThisAttack = new List<GameObject>();
    }

}
