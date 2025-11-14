using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    private InputSystem_Actions mInputAction;

    [Header("Movement Settings")]
    [SerializeField] private float jumpSpeed = 6f;
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float groundAcceleration = 40f;
    [SerializeField] private float airAcceleration = 5f;
    [SerializeField] private float turnLerpRate = 40f;
    [SerializeField] private float maxFallSpeed = 50f;
    [SerializeField] private float airCheckRadius = 0.2f;
    [SerializeField] private LayerMask airCheckLayerMask = 1;

    private CharacterController characterController;
    private Animator animator;
    private Camera mainCamera;

    [Header("Player Interaction")]
     private Collider mInteractableInRange;

    private CharacterController mCharacterController;
    private Animator mAnimator;
    private Vector3 verticalVelocity;
    private Vector3 horizontalVelocity;
    private Vector2 moveInput;
    private bool shouldJump;
    private bool isInAir;
    private bool mCanAttack;


    private GameObject currentTarget;

    public GameObject CurrentTarget => currentTarget;

    void Awake()
    {

        mInputAction = new InputSystem_Actions();
        mInputAction.Player.Jump.performed += PerformJump;
        mInputAction.Player.Move.performed += HandleMoveInput;
        mInputAction.Player.Move.canceled += HandleMoveInput;
        mInputAction.Player.Attack.performed += ctx => TryMagicAttack();
        mInputAction.Player.Interact.performed += ctx => TryInteraction();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable() => mInputAction.Enable();
    private void OnDisable() => mInputAction.Disable();

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void PerformJump(InputAction.CallbackContext context)
    {
        if (!isInAir && context.performed)
        {
            shouldJump = true;
        }
    }
    private void TryMagicAttack()
    {
        if (mCanAttack && mMagicAttackPrefab != null && mMagicAttackSpawn != null)
        {
            mCanAttack = false;
            mAnimator.SetTrigger("Attack");

            GameObject magicClone = Instantiate(mMagicAttackPrefab, mMagicAttackSpawn.position, mMagicAttackSpawn.rotation);
            Rigidbody rBody = magicClone.GetComponent<Rigidbody>();
            if (rBody != null)
            {
                rBody.AddForce(mMagicAttackSpawn.forward * mMagicForce, ForceMode.Impulse);
            }

            Invoke(nameof(ResetAttack), 1.0f); // Cooldown or animation delay
        }
    }
    private void TryInteraction()
    {
         if (mInteractableInRange != null)
        {
            Lever lever = mInteractableInRange.GetComponent<Lever>();
            if (lever != null)
            {
                lever.Activate();
            }
            Door door = mInteractableInRange.GetComponent<Door>();
            if (door != null)
            {
                door.Activate();
            }
        }
    }
     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            mInteractableInRange = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == mInteractableInRange)
        {
            mInteractableInRange = null;
        }
    }

    private void ResetAttack()
    {
        mCanAttack = true;
    }


    void Update()
    {
        isInAir = IsInAir();

        UpdateVerticalVelocity();
        UpdateHorizontalVelocity();
        UpdateTransform();
        UpdateAnimation();
        HandleMouseTargeting();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("Speed", horizontalVelocity.magnitude);
        animator.SetBool("Landed", !isInAir);
    }

    private void UpdateTransform()
    {
        characterController.Move((horizontalVelocity + verticalVelocity) * Time.deltaTime);
        if (horizontalVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(horizontalVelocity.normalized, Vector3.up),
                Time.deltaTime * turnLerpRate);
        }
    }

    private void UpdateVerticalVelocity()
    {
        if (shouldJump && !isInAir)
        {
            verticalVelocity.y = jumpSpeed;
            animator.SetTrigger("Jump");
            shouldJump = false;
            return;
        }

        if (characterController.isGrounded)
        {
            animator.ResetTrigger("Jump");
            verticalVelocity.y = -1f;
            return;
        }

        if (verticalVelocity.y > -maxFallSpeed)
        {
            verticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void UpdateHorizontalVelocity()
    {
        Vector3 moveDir = PlayerInputToWorldDir(moveInput);
        float acceleration = characterController.isGrounded ? groundAcceleration : airAcceleration;

        if (moveDir.sqrMagnitude > 0)
        {
            horizontalVelocity += moveDir * acceleration * Time.deltaTime;
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxMoveSpeed);
        }
        else
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }
    }

    private Vector3 PlayerInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = mainCamera.transform.right;
        Vector3 fwdDir = Vector3.Cross(rightDir, Vector3.up);
        return rightDir * inputVal.x + fwdDir * inputVal.y;
    }

    private bool IsInAir()
    {
        if (characterController.isGrounded) return false;

        Collider[] airCheckColliders = Physics.OverlapSphere(transform.position, airCheckRadius, airCheckLayerMask);
        foreach (Collider collider in airCheckColliders)
        {
            if (collider.gameObject != gameObject) return false;
        }
        return true;
    }

    private void HandleMouseTargeting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    currentTarget = hit.collider.gameObject;
                    Debug.Log("Targeted Enemy: " + currentTarget.name);
                }
                else
                {
                    currentTarget = null;
                    Debug.Log("Target cleared.");
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isInAir ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, airCheckRadius);
    }

}
