using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnStart;
    [SerializeField] private float delay;
    [SerializeField] private UnityEvent onTriggerEvent;

    private void Start()
    {
        if(triggerOnStart) { TriggerWithDelay(delay); }
    }

    public void Trigger() => onTriggerEvent?.Invoke();

    public void TriggerWithDelay(float delay) => Invoke(nameof(Trigger), delay);
}
