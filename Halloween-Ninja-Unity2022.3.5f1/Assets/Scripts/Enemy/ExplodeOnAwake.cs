using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnAwake : MonoBehaviour
{
    [SerializeField] Rigidbody[] pieces;
    [SerializeField] float explosionForce = 100;
    [SerializeField] Vector3 explosionOffset;
    [SerializeField] float explosionRadius = 3;
    [SerializeField] float destroyAfter = 5f;

    private void Awake()
    {
        foreach (Rigidbody rb in pieces)
        {
            rb.AddExplosionForce(explosionForce, transform.position + explosionOffset, explosionRadius);
            Destroy(gameObject, destroyAfter);
        }
    }
}
