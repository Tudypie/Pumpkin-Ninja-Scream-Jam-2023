using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    [Tooltip("Straddle 0")]
    [SerializeField] AnimationCurve flickerCurve;
    [Tooltip("Number of seconds for each pass through the flickerCurve")]
    [SerializeField] float flickerSpeed;
    [SerializeField] float flickerMagnitude;
    Light objectLight;
    float baseIntensity;
    float currentCurvePosition;
    [SerializeField] bool randomizeStartingValue;

    private void Awake()
    {
        objectLight = GetComponent<Light>();
        baseIntensity = objectLight.intensity;
        if(randomizeStartingValue) currentCurvePosition = Random.Range(0f, 1f);
    }

    private void Update()
    {
        currentCurvePosition += Time.deltaTime / flickerSpeed;
        if (currentCurvePosition > 1) currentCurvePosition--;



        objectLight.intensity = baseIntensity + flickerCurve.Evaluate(currentCurvePosition) * flickerMagnitude;
    }


}
