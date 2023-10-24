using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenTag : MonoBehaviour
{
    public bool tagged = false;
    float tagDuration = 5;
    float tagExpiryTime;
    public void Tag()
    {
        tagged = true;
        tagExpiryTime = Time.time + tagDuration;
    }

    public void Update()
    {
        if (Time.time > tagExpiryTime) tagged = false;
    }
}
