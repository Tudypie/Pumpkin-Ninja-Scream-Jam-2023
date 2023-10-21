using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Interactable : MonoBehaviour
{
    public bool ableToInteract = true;
    [SerializeField] private string interactMessage;
    [SerializeField] private UnityEvent interactEvent;

    private bool rayHitObject = false;

    private InteractionController interactionController;
    private Collider coll;

    private void Awake()
    {
        interactionController = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionController>();
        coll = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        interactionController.OnHitSomething += CheckInteraction;
    }

    private void OnDisable()
    {
        interactionController.OnHitSomething -= CheckInteraction;
    }

    private void Update()
    {
        if (!rayHitObject || !ableToInteract) { return; }

        if (Input.GetKeyDown(interactionController.interactKey))
            interactEvent?.Invoke();
    }

    private void CheckInteraction(RaycastHit hit)
    {
        rayHitObject = hit.collider == coll;
        interactionController.interactText.text = rayHitObject ? interactMessage : "";
    }
}