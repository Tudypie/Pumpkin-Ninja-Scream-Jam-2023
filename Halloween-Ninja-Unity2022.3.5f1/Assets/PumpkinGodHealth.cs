using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PumpkinGodhealth : MonoBehaviour
{
    [SerializeField] TMP_Text healthText;
    Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.OnTakeDamage += TakeDamage;
        health.OnDeath += PumpkinGodDeath;
    }

    void TakeDamage(object sender, EventArgs e)
    {
        healthText.text = health.currenthealth + "%";
    }

    void PumpkinGodDeath(object sender, EventArgs e)
    {
        LoseSystem.Instance.Win = true;
    }

    
}
