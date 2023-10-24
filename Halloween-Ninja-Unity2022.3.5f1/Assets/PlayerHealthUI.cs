using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Health playerhealth;
    [SerializeField] Health PumpkinGodHealth;

    // Start is called before the first frame update
    void Start()
    {
        PumpkinGodHealth.OnDeath += EnableHealth;
        playerhealth.enabled = true;
        playerhealth.OnDeath += LoseGame;
    }

    void EnableHealth(object sender, EventArgs e) 
    {
        playerhealth.currenthealth = 100;
        text.gameObject.SetActive(true);
    }

    void LoseGame(object sender, EventArgs e)
    {
        LoseSystem.Instance.Defeat();
    }


    private void Update()
    {
        text.text = playerhealth.currenthealth.ToString() + "%";
    }

}
