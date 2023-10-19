using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerSize : MonoBehaviour
{
    [Tooltip("Straddle 0")]
    [SerializeField] AnimationCurve flickerCurve;
    [Tooltip("Number of seconds for each pass through the flickerCurve")]
    [SerializeField] float flickerSpeed;
    [SerializeField] float flickerMagnitude;
    Vector3 baseScale;
    float currentCurvePosition;

    [SerializeField] bool randomizeStartingValue;

    private void Awake()
    {
        baseScale = transform.localScale;
        if (randomizeStartingValue) currentCurvePosition = Random.Range(0f, 1f);
    }

    private void Update()
    {
        currentCurvePosition += Time.deltaTime/flickerSpeed;
        if (currentCurvePosition > 1) currentCurvePosition--;



        transform.localScale = baseScale + baseScale * flickerCurve.Evaluate(currentCurvePosition) * flickerMagnitude;
    }


}
