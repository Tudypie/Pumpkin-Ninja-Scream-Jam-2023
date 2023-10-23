using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public EventHandler OnDeath;
    public class DamageEventArgs : EventArgs {
        float damageTaken;
        public DamageEventArgs(float damageTaken)
        {
            this.damageTaken = damageTaken;
        }
        }
    public EventHandler<DamageEventArgs> OnTakeDamage;
    public class HealingEventArgs : EventArgs
    {
        float amountHealed;
        public HealingEventArgs(float amountHealed)
        {
            this.amountHealed = amountHealed;
        }
    }
    public EventHandler<HealingEventArgs> OnHealing;

    public int maxhealth;
    public float currenthealth;
    [SerializeField] private bool destroyOnDeath = true;

    public void Awake()
    {
        currenthealth = maxhealth;
    }

    public void Heal(float healAmount)
    {
        currenthealth = Mathf.Clamp(currenthealth += healAmount, 0, maxhealth);
        OnHealing?.Invoke(this, new Health.HealingEventArgs(healAmount));
    }

    public void TakeDamage(float damage)
    {
        currenthealth = Mathf.Clamp(currenthealth - damage, 0, maxhealth);
        OnTakeDamage?.Invoke(this, new Health.DamageEventArgs(damage));

        if (currenthealth <= 0) Death();
    }

    public void Death()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);

        if (destroyOnDeath)
            Destroy(gameObject);

        enabled = false;
    }
}
