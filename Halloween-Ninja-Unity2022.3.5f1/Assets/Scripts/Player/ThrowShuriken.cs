using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowShuriken : MonoBehaviour
{
    [Header("Spawning Parameters")]
    [SerializeField] private GameObject shurikenInHand;
    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private float throwForce = 500f;  
    [SerializeField] private float throwDelay = 0.5f;
    private float currentThrowDelay = 0.0f;

    [Header("Controls")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (currentThrowDelay > 0)
        {
            currentThrowDelay -= Time.deltaTime;
            return;
        }
        else
        {
            shurikenInHand.SetActive(true);
        }

        if (Input.GetKey(throwKey))
        {
            shurikenInHand.SetActive(false);

            Vector3 throwDirection = mainCam.transform.forward;

            GameObject shuriken = Instantiate(shurikenPrefab, spawnpoint.position , mainCam.transform.localRotation);
            Rigidbody shurikenRigidbody = shuriken.GetComponent<Rigidbody>();

            if (shurikenRigidbody != null)
            {
                shurikenRigidbody.AddForce(throwDirection * throwForce);
            }

            currentThrowDelay = throwDelay;
        }
    }
}
