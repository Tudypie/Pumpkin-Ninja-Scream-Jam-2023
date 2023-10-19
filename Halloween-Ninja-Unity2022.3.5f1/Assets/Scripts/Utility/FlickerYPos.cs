using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerYPos : MonoBehaviour
{
    [Tooltip("Straddle 0")]
    [SerializeField] AnimationCurve flickerCurve;
    [Tooltip("Number of seconds for each pass through the flickerCurve")]
    [SerializeField] float flickerSpeed;
    [SerializeField] float flickerMagnitude;
    float baseY;
    float currentCurvePosition;

    [SerializeField] bool randomizeStartingValue;

    private void Awake()
    {
        baseY = transform.localPosition.y;
        if (randomizeStartingValue) currentCurvePosition = Random.Range(0f, 1f);
    }

    private void Update()
    {
        currentCurvePosition += Time.deltaTime / flickerSpeed;
        if (currentCurvePosition > 1) currentCurvePosition--;

        float newY = baseY + flickerCurve.Evaluate(currentCurvePosition) * flickerMagnitude;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

}
