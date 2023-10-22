using System.Collections;
using UnityEngine;
using FMODUnity;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    public bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    public bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool useSprintZoom = true;
    [SerializeField] private bool useHeadbob = true;
    [SerializeField] private bool useFootsteps = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 3.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14.0f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18.0f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    private float defaultYPos = 0;
    private float timer;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 45f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    [Header("Sprint Zoom Parameters")]
    [SerializeField] private float sprintZoomFOV = 90f;
    [SerializeField] private float sprintZoomSmooth = 0.05f;


    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    private float footstepTimer = 0f;
    private float GetCurrentOffset => IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(!CanMove) { return; }

        HandleMovementInput();
        HandleMouseLook();

        if(canJump) { HandleJump(); }
        if(useHeadbob) { HandleHeadBob(); }
        if(canZoom) { HandleZoom(); }
        if(useSprintZoom) { HandleSprintZoom(); }
        if(useFootsteps) { HandleFootsteps(); }

        ApplyFinalMovement();
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(
            (IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxisRaw("Vertical"),
            (IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxisRaw("Horizontal")
            );

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
            (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
        //moveDirection.magnitude = Mathf.Clamp(moveDirection.magnitude, 0, IsSprinting ? sprintSpeed : walkSpeed);
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
         if(ShouldJump)
        {
            moveDirection.y = jumpForce;
            FMODAudio.Instance.PlayAudio(FMODAudio.Instance.characterJump, transform.position);
        }
    }

    private void HandleZoom()
    {
        if(Input.GetKeyDown(zoomKey))
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }

        if (Input.GetKeyUp(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleSprintZoom()
    {
        if(zoomRoutine != null || playerCamera.fieldOfView == zoomFOV) { return; }

        if (currentInput != Vector2.zero)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintZoomFOV, sprintZoomSmooth);
        }

        if (currentInput == Vector2.zero)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, sprintZoomSmooth);
        }
    }

    private void HandleHeadBob()
    {
        if(!characterController.isGrounded) { return; }

        if(Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3 (
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z
                );
        }
    }

    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) { return; }

        if (currentInput == Vector2.zero) { return; }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                FMODAudio.Instance.PlayAudio(FMODAudio.Instance.characterFootsteps, transform.position);
            }
            footstepTimer = GetCurrentOffset;
        }
    }

    private void ApplyFinalMovement()
    {
        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
    
    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while(timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }
}
