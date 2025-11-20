using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.ShaderData;

public class SMagicAttackController : MonoBehaviour
{
    [Header("Magic Attack Settings")]
    //[SerializeField] private List<SMagicAttackData> magicAttacks;// All attack types
    
    //Testing for getter and setter
    [field:SerializeField]
    public List<SMagicAttackData> magicAttacks { get; private set; }

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

        inputActions.Player.SwitchAttackKey.performed += ctx =>
        {
            var KeyControl = ctx.control as KeyControl;
            SwitchAttackKey(KeyControl);

        };

        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
    }

    private void SwitchAttackKey(KeyControl KeyControl)
    {
        switch (KeyControl.keyCode)
        {
            case Key.Digit1:
                CycleAttackKey(1);
                break;
            case Key.Digit2:
                CycleAttackKey(2);
                break;
            case Key.Digit3:
                CycleAttackKey(3);
                break;
            case Key.Digit4:
                CycleAttackKey(4);
                break;
            case Key.Digit5:
                CycleAttackKey(5);
                break;
            default:
                break;
        }
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void CycleAttackKey(int index) 
    {
        if (index >= magicAttacks.Count + 1) { return; }
        currentAttackIndex = (index - 1);
        Debug.Log($"Switched to attack: {magicAttacks[currentAttackIndex].mAttackName}");
    }
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
        // Pass freeze info to projectile if needed
        SProjectileLogic explosionScript = magicClone.GetComponent<SProjectileLogic>();
        if (explosionScript != null)
        {
            explosionScript.isFreezeAttack = attackData.mIsFreezeAttack;
            explosionScript.mSpecialEffectPrefab = attackData.mSpecialEffectPrefab;
            explosionScript.mEffectDuration = attackData.mEffectDuration;
        }

        Rigidbody rBody = magicClone.GetComponent<Rigidbody>();
        if (rBody != null)
        {
            Vector3 direction = mCurrentTarget != null
                ? (mCurrentTarget.transform.position - magicAttackSpawn.position).normalized
                : magicAttackSpawn.forward;

            rBody.AddForce(direction * attackData.mForce, ForceMode.Impulse);
        }
    }

}

