using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnOnDeath : MonoBehaviour
{
    [SerializeField] Transform bodyToReplace;
    [SerializeField] Transform bodyToReplaceWith;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().OnDeath += SpawnDeadPrefab;
    }

    void SpawnDeadPrefab(object sender, EventArgs e)
    {
        Transform newBody = Instantiate(bodyToReplaceWith, bodyToReplace.position, bodyToReplace.rotation);
        Destroy(bodyToReplace.gameObject);      
    }

}
