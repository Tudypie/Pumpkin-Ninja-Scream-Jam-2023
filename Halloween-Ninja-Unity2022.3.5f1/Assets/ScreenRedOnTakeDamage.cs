using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ScreenRedOnTakeDamage : MonoBehaviour
{
    [SerializeField] Image redUI;
    [SerializeField] Health PumpkinGodHealth;
    // Start is called before the first frame update
    void Start()
    {
        PumpkinGodHealth.OnDeath += EnableHealth;
    }

    void EnableHealth(object sender, EventArgs e)
    {
        GetComponent<Health>().OnTakeDamage += OnTakeDamage;
    }

    void OnTakeDamage(object sender, EventArgs e)
    {
        redUI.color = Color.white;
    }

    private void Update()
    {
        redUI.color = Color.Lerp(redUI.color, Color.clear, 2 * Time.deltaTime);
    }

}
