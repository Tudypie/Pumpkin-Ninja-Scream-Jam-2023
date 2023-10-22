using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerRotation : MonoBehaviour
{
    Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float playerYRotation = playerTransform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, playerYRotation, transform.eulerAngles.z);
    }
}
