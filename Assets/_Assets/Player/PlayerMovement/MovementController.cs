using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    InputSystem_Actions mInputAction;

    [Header("Movement Settings")]
    [SerializeField] float mJumpSpeed = 6f;
    [SerializeField] float mMaxMoveSpeed = 5f;
    [SerializeField] float mGroundMoveSpeedAcceleration = 40f;
    [SerializeField] float mAirMoveSpeedAcceleration = 5f;
    [SerializeField] float mTurnLerpRate = 40f;
    [SerializeField] float mMaxFallSpeed = 50f;
    [SerializeField] float mAirCheckRadius = 0.2f;
    [SerializeField] LayerMask mAirCheckLayerMask = 1;

    [Header("Magic Attack Settings")]
    [SerializeField] private GameObject mMagicAttackPrefab;
    [SerializeField] private Transform mMagicAttackSpawn;
    [SerializeField] private float mMagicForce = 20.0f;

    [Header("Player Interaction")]
     private Collider mInteractableInRange;

    private CharacterController mCharacterController;
    private Animator mAnimator;

    private Vector3 mVerticalVelocity;
    private Vector3 mHorizontalVelocity;
    private Vector2 mMoveInput;

    private bool mShouldTryJump;
    private bool mIsInAir;
    private bool mCanAttack = true;

    public InputSystem_Actions GetInputActions()
    {
        return mInputAction;
    }

    void Awake()
    {
        mInputAction = new InputSystem_Actions();
        mInputAction.Player.Jump.performed += PerformJump;
        mInputAction.Player.Move.performed += HandleMoveInput;
        mInputAction.Player.Move.canceled += HandleMoveInput;
        mInputAction.Player.Attack.performed += ctx => TryMagicAttack();
        mInputAction.Player.Interact.performed += ctx => TryInteraction();

        mCharacterController = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        mInputAction.Enable();
    }

    private void OnDisable()
    {
        mInputAction.Disable();
    }

    public void HandleMoveInput(InputAction.CallbackContext context)
    {
        mMoveInput = context.ReadValue<Vector2>();
    }

    public void PerformJump(InputAction.CallbackContext context)
    {
        if (!mIsInAir)
        {
            mShouldTryJump = true;
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
        mIsInAir = IsInAir();

        UpdateVerticalVelocity();
        UpdateHorizontalVelocity();
        UpdateTransform();
        UpdateAnimation();
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mHorizontalVelocity.normalized, Vector3.up),
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

    void UpdateHorizontalVelocity()
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
            if (mHorizontalVelocity.sqrMagnitude > 0)
            {
                mHorizontalVelocity -= mHorizontalVelocity.normalized * acceleration * Time.deltaTime;
                if (mHorizontalVelocity.sqrMagnitude < 0.1)
                {
                    mHorizontalVelocity = Vector3.zero;
                }
            }
        }
    }

    Vector3 PlayerInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = Camera.main.transform.right;
        Vector3 fwdDir = Vector3.Cross(rightDir, Vector3.up);
        return rightDir * inputVal.x + fwdDir * inputVal.y;
    }

    bool IsInAir()
    {
        if (mCharacterController.isGrounded)
        {
            return false;
        }

        Collider[] airCheckColliders = Physics.OverlapSphere(transform.position, mAirCheckRadius, mAirCheckLayerMask);
        foreach (Collider collider in airCheckColliders)
        {
            if (collider.gameObject != gameObject)
            {
                return false;
            }
        }

        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = mIsInAir ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, mAirCheckRadius);
    }
}
