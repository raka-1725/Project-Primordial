using UnityEngine;

public class SMagicAttackController : MonoBehaviour
{
    [Header("Magic Attack Settings")]
    [SerializeField] private GameObject magicAttackPrefab;
    [SerializeField] private Transform magicAttackSpawn;
    [SerializeField] private float magicForce = 20f;

    private InputSystem_Actions inputActions;
    private Animator animator;
    private MovementController movementController;
    private bool canAttack = true;

    private GameObject mCurrentTarget;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Attack.performed += ctx => TryMagicAttack();

        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void TryMagicAttack()
    {
        mCurrentTarget = movementController.CurrentTarget;
        if (canAttack && mCurrentTarget != null && magicAttackPrefab && magicAttackSpawn)
        {
            canAttack = false;
            animator.SetTrigger("Attack");

            Invoke(nameof(ResetAttack), 1f);
        }
    }

    private void SpawnMagic()
    {
        GameObject magicClone = Instantiate(magicAttackPrefab, magicAttackSpawn.position, magicAttackSpawn.rotation);
        Rigidbody rBody = magicClone.GetComponent<Rigidbody>();
        if (rBody != null)
        {
            Vector3 direction = (mCurrentTarget.transform.position - magicAttackSpawn.position).normalized;
            rBody.AddForce(direction * magicForce, ForceMode.Impulse);
        }
    }

    private void ResetAttack() => canAttack = true;
}

