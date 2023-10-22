using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{
    [SerializeField] private int maxCombo = 16;
    [SerializeField] private int currentCombo = 1;
    [Space]
    [SerializeField] private float comboTimer = 5.0f;
    [SerializeField] private float comboTimerAdd = 2.0f;
    [SerializeField] private float currentComboTimer;
    [Space]
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private Image comboFillCircle;

    public static ComboSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }

        currentComboTimer = 0;
    }

    private void Update()
    {
        comboText.text = "x" + currentCombo; 
        comboFillCircle.fillAmount = (currentComboTimer / comboTimer);

        currentCombo = Mathf.Min(currentCombo, maxCombo);

        if(currentComboTimer > 0)
        {
            currentComboTimer -= Time.deltaTime;
        }
        else if(currentComboTimer <= 0)
        {
            DecreaseCombo();
        }
    }

    public void AddCombo()
    {
        currentComboTimer += comboTimerAdd;

        if (currentComboTimer >= comboTimer && currentCombo < maxCombo)
        {
            currentCombo *= 2;
            currentComboTimer = comboTimer / 2;
        }
    }

    public void DecreaseCombo()
    {
        if (currentCombo > 1)
        {
            currentCombo /= 2;
            currentComboTimer = comboTimer - 0.1f;
        }
    }
}
