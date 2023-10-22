using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DefensePoint : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;

    private Interactable interactable;
    private Health health;

    void Awake()
    {
        interactable = GetComponent<Interactable>();
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += DefensePointDeath;
        health.OnTakeDamage += DefensePointTakeDamage;
    }

    private void OnDisable()
    {
        health.OnDeath -= DefensePointDeath;
        health.OnTakeDamage -= DefensePointTakeDamage;
    }

    void Update()
    {
        hpText.text = health.currenthealth + "%";
    }

    private void DefensePointTakeDamage(object sender, EventArgs e)
    {
        ComboSystem.Instance.DecreaseCombo();
    }

    private void DefensePointDeath(object sender, EventArgs e)
    {
        LoseGame.Instance.Lose = true;
    }

    public void StartWave()
    {
        WaveSystem.Instance.NewWave();
        interactable.ableToInteract = false;
    }
}
