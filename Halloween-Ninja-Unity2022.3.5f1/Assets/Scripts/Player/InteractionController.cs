using System;
using UnityEngine;
using TMPro;

public class InteractionController : MonoBehaviour
{
    [Header("Interaction Parameters")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI")]
    public TMP_Text interactText;

    [Header("Controls")]
    public KeyCode interactKey;

    [Header("Raycast")]
    public bool hitSomething;
    public event Action<RaycastHit> OnHitSomething;
    public RaycastHit hitInfo;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        if (!Physics.Raycast(ray, out hitInfo, interactDistance, interactableLayer))
        {
            interactText.text = "";
            return;
        }

        Debug.Log("raycast");

        hitInfo.collider.gameObject.TryGetComponent(out Interactable interactable);

        if(interactable == null || !interactable.ableToInteract)
        {
            interactText.text = "";
            return;
        }

        OnHitSomething?.Invoke(hitInfo);
    }
}