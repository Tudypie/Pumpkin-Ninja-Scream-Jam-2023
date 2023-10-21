using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnDamage : MonoBehaviour
{
    Health health;
    [SerializeField] Renderer[] renderers;
    Material[][] materials;
    float endFlashTime;

    [SerializeField] Material flashMaterial;
    [SerializeField] float flashDuration = 0.25f;
    bool isFlashing = false;

    private void Start()
    {
        health = GetComponent<Health>();

        materials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            materials[i] = renderer.materials;
        }
        health.OnTakeDamage += Health_OnTakeDamage;
    }

    private void Health_OnTakeDamage(object sender, Health.DamageEventArgs e)
    {
        StartFlash();
    }

    private void Update()
    {
        if (!isFlashing) return;

        if (Time.time > endFlashTime) ResetMaterials();
    }

    void StartFlash() 
    {
        isFlashing = true;
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];

            Material[] flashMaterials = new Material[renderer.materials.Length];
            for (int j = 0; j < flashMaterials.Length; j++)
            {
                flashMaterials[j] = flashMaterial;
            }

            renderer.materials = flashMaterials;
        }

        endFlashTime = Time.time + flashDuration;
    }

    void ResetMaterials()
    {
        isFlashing = false;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = materials[i];
        }
    }



}
