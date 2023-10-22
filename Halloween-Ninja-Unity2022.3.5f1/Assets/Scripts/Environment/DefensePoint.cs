using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DefensePoint : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;

    private Transform healthPointsParent;
    private GameObject healthPointImagePrefab;
    private List<GameObject> healthPointImages = new List<GameObject>();

    private Interactable interactable;
    private Health health;

    void Awake()
    {
        interactable = GetComponent<Interactable>();
        health = GetComponent<Health>();
        /*for(int i = 0; i < health.currenthealth; i++)
        {
            healthPointImages.Add(Instantiate(healthPointImagePrefab, healthPointsParent));
        }*/
    }

    void Update()
    {
        hpText.text = health.currenthealth + "%";
    }

    private void UpdateHealthPointImages()
    {
        int i;
        for(i = 0; i < health.currenthealth; i++)
        {
            healthPointImages[i].gameObject.SetActive(true);
        }

        int j;
        for (j = i; j < health.maxhealth; j++)
        {
            healthPointImages[j].gameObject.SetActive(false);
        }
    }

    public void StartWave()
    {
        WaveSystem.Instance.NewWave();
        interactable.ableToInteract = false;
    }
}
