using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    // Input System
    private InputSystem_Actions mInputAction;

    [Header("Movement Settings")]
    [SerializeField] private float mJumpSpeed = 6f;
    [SerializeField] private float mMaxMoveSpeed = 5f;
    [SerializeField] private float mGroundMoveSpeedAcceleration = 40f;
    [SerializeField] private float mAirMoveSpeedAcceleration = 5f;
    [SerializeField] private float mTurnLerpRate = 40f;
    [SerializeField] private float mMaxFallSpeed = 50f;
    [SerializeField] private float mAirCheckRadius = 0.2f;
    [SerializeField] private LayerMask mAirCheckLayerMask = 1;

    [Header("Magic Attack Settings")]
    [SerializeField] private GameObject mMagicAttackPrefab;
    [SerializeField] private Transform mMagicAttackSpawn;
    [SerializeField] private float mMagicForce = 20.0f;

    // Components 
    private CharacterController mCharacterController;
    private Animator mAnimator;
    private Camera mainCamera;

    // Movement State
    private Vector3 mVerticalVelocity;
    private Vector3 mHorizontalVelocity;
    private Vector2 mMoveInput;
    private bool mShouldTryJump;
    private bool mIsInAir;

    // Attack State 
    private bool mCanAttack = true;
    private GameObject currentTarget = null;

    void Awake()
    {
        mInputAction = new InputSystem_Actions();
        mInputAction.Player.Jump.performed += PerformJump;
        mInputAction.Player.Move.performed += HandleMoveInput;
        mInputAction.Player.Move.canceled += HandleMoveInput;
        mInputAction.Player.Attack.performed += ctx => TryMagicAttack();

        mCharacterController = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable() => mInputAction.Enable();
    private void OnDisable() => mInputAction.Disable();

    public void HandleMoveInput(InputAction.CallbackContext context)
    {
        mMoveInput = context.ReadValue<Vector2>();
    }

    public void PerformJump(InputAction.CallbackContext context)
    {
        if (!mIsInAir && context.performed)
        {
            mShouldTryJump = true;
        }
    }

    private void TryMagicAttack()
    {
        if (mCanAttack && currentTarget != null && mMagicAttackPrefab != null && mMagicAttackSpawn != null)
        {
            mCanAttack = false;
            mAnimator.SetTrigger("Attack");

            GameObject magicClone = Instantiate(mMagicAttackPrefab, mMagicAttackSpawn.position, mMagicAttackSpawn.rotation);
            Rigidbody rBody = magicClone.GetComponent<Rigidbody>();
            if (rBody != null)
            {
                // Aim toward target
                Vector3 direction = (currentTarget.transform.position - mMagicAttackSpawn.position).normalized;
                rBody.AddForce(direction * mMagicForce, ForceMode.Impulse);
            }

            Invoke(nameof(ResetAttack), 1.0f);
        }
    }

    private void ResetAttack() => mCanAttack = true;

    void Update()
    {
        mIsInAir = IsInAir();

        UpdateVerticalVelocity();
        UpdateHorizontalVelocity();
        UpdateTransform();
        UpdateAnimation();
        HandleMouseTargeting();
    }

    private void UpdateAnimation()
    {
        mAnimator.SetFloat("Speed", mHorizontalVelocity.magnitude);
        mAnimator.SetBool("Landed", !mIsInAir);
    }

    private void UpdateTransform()
    {
        mCharacterController.Move((mHorizontalVelocity + mVerticalVelocity) * Time.deltaTime);
        if (mHorizontalVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(mHorizontalVelocity.normalized, Vector3.up),
                Time.deltaTime * mTurnLerpRate);
        }
    }

    private void UpdateVerticalVelocity()
    {
        if (mShouldTryJump && !mIsInAir)
        {
            mVerticalVelocity.y = mJumpSpeed;
            mAnimator.SetTrigger("Jump");
            mShouldTryJump = false;
            return;
        }

        if (mCharacterController.isGrounded)
        {
            mAnimator.ResetTrigger("Jump");
            mVerticalVelocity.y = -1f;
            return;
        }

        if (mVerticalVelocity.y > -mMaxFallSpeed)
        {
            mVerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void UpdateHorizontalVelocity()
    {
        Vector3 moveDir = PlayerInputToWorldDir(mMoveInput);
        float acceleration = mCharacterController.isGrounded ? mGroundMoveSpeedAcceleration : mAirMoveSpeedAcceleration;

        if (moveDir.sqrMagnitude > 0)
        {
            mHorizontalVelocity += moveDir * acceleration * Time.deltaTime;
            mHorizontalVelocity = Vector3.ClampMagnitude(mHorizontalVelocity, mMaxMoveSpeed);
        }
        else
        {
            mHorizontalVelocity = Vector3.MoveTowards(mHorizontalVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }
    }

    Vector3 PlayerInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = mainCamera.transform.right;
        Vector3 fwdDir = Vector3.Cross(rightDir, Vector3.up);
        return rightDir * inputVal.x + fwdDir * inputVal.y;
    }

    bool IsInAir()
    {
        if (mCharacterController.isGrounded) return false;

        Collider[] airCheckColliders = Physics.OverlapSphere(transform.position, mAirCheckRadius, mAirCheckLayerMask);
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
        Gizmos.color = mIsInAir ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, mAirCheckRadius);
    }

}
