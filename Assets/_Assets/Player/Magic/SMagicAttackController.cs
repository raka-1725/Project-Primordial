using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class SMagicAttackController : MonoBehaviour
{
    [Header("Magic Attack Settings")]
    [SerializeField] private List<SMagicAttackData> magicAttacks; // All attack types
    [SerializeField] private Transform magicAttackSpawn;

    private InputSystem_Actions inputActions;
    private Animator animator;
    private MovementController movementController;

    private int currentAttackIndex = 0;
    private float nextAttackTime = 0f;
    private GameObject mCurrentTarget;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Attack.performed += ctx => TryMagicAttack();

        inputActions.Player.SwitchAttackScroll.performed += ctx =>
        {
            Vector2 scrollValue = ctx.ReadValue<Vector2>();
            if (scrollValue.y > 0) CycleAttack(1);
            else if (scrollValue.y < 0) CycleAttack(-1);
        };

        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
    private void CycleAttack(int direction)
    {
        currentAttackIndex += direction;
        if (currentAttackIndex >= magicAttacks.Count) currentAttackIndex = 0;
        if (currentAttackIndex < 0) currentAttackIndex = magicAttacks.Count - 1;

        Debug.Log($"Switched to attack: {magicAttacks[currentAttackIndex].mAttackName}");
    }

    private void TryMagicAttack()
    {
        mCurrentTarget = movementController.CurrentTarget;
        if (mCurrentTarget == null || magicAttacks.Count == 0) return;

        SMagicAttackData attackData = magicAttacks[currentAttackIndex];

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackData.mCooldown;
        }
    }

    // Called by animation event
    private void SpawnMagic()
    {
        SMagicAttackData attackData = magicAttacks[currentAttackIndex];
        GameObject magicClone = Instantiate(attackData.mAttackPrefab, magicAttackSpawn.position, magicAttackSpawn.rotation);

        Rigidbody rBody = magicClone.GetComponent<Rigidbody>();
        if (rBody != null)
        {
            Vector3 direction = (mCurrentTarget.transform.position - magicAttackSpawn.position).normalized;
            rBody.AddForce(direction * attackData.mForce, ForceMode.Impulse);
        }
    }

}

