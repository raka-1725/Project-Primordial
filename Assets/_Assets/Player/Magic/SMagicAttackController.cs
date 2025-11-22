using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.ShaderData;

public class SMagicAttackController : MonoBehaviour
{
    [Header("Magic Attack Settings")]
    [field: SerializeField] public List<SMagicAttackData> magicAttacks { get; private set; }
    [SerializeField] private Transform magicAttackSpawn;

    private InputSystem_Actions inputActions;
    private Animator animator;
    private MovementController movementController;
    //UI
    private MagicSelector mMagicSelector;

    private int currentAttackIndex = 0;
    private GameObject mCurrentTarget;
    private float lastAttackTime = 0f;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Attack.performed += ctx => TryAttack();

        inputActions.Player.SwitchAttackScroll.performed += ctx =>
        {
            Vector2 scrollValue = ctx.ReadValue<Vector2>();
            if (scrollValue.y > 0) CycleAttack(1);
            else if (scrollValue.y < 0) CycleAttack(-1);

        };

        inputActions.Player.SwitchAttackKey.performed += ctx =>
        {
            var keyControl = ctx.control as KeyControl;
            SwitchAttackKey(keyControl);
        };

        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
        mMagicSelector = FindAnyObjectByType<MagicSelector>();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void SwitchAttackKey(KeyControl keyControl)
    {
        switch (keyControl.keyCode)
        {
            case UnityEngine.InputSystem.Key.Digit1: CycleAttackKey(1); break;
            case UnityEngine.InputSystem.Key.Digit2: CycleAttackKey(2); break;
            case UnityEngine.InputSystem.Key.Digit3: CycleAttackKey(3); break;
            case UnityEngine.InputSystem.Key.Digit4: CycleAttackKey(4); break;
            case UnityEngine.InputSystem.Key.Digit5: CycleAttackKey(5); break;
        }
    }

    private void CycleAttackKey(int index)
    {
        if (index < 1 || index > magicAttacks.Count) return;
        currentAttackIndex = index - 1;
        mMagicSelector.UpdateCycleAttackIndex(currentAttackIndex);
        Debug.Log($"Switched to attack: {magicAttacks[currentAttackIndex].mAttackName}");
    }

    private void CycleAttack(int direction)
    {
        currentAttackIndex += direction;
        if (currentAttackIndex >= magicAttacks.Count) currentAttackIndex = 0;
        if (currentAttackIndex < 0) currentAttackIndex = magicAttacks.Count - 1;
        mMagicSelector.UpdateCycleAttackIndex(currentAttackIndex);
        Debug.Log($"Switched to attack: {magicAttacks[currentAttackIndex].mAttackName}");
    }

    private void TryAttack()
    {
        mCurrentTarget = movementController.CurrentTarget;
        if (mCurrentTarget == null || magicAttacks.Count == 0) return;

        SMagicAttackData attackData = magicAttacks[currentAttackIndex];

        // Cooldown check
        if (Time.time < lastAttackTime + attackData.mCooldown) 
        {
            float timeLeft = (lastAttackTime + attackData.mCooldown) - Time.time;
            float normalized = timeLeft / attackData.mCooldown;

            mMagicSelector.onAttackCoolDown(currentAttackIndex, normalized);
            return;
                
        }
        lastAttackTime = Time.time;

        switch (attackData.mAnimationType)
        {
            case AttackAnimationType.Magic:
                animator.SetTrigger("Attack");
                break;
            case AttackAnimationType.AoE:
                animator.SetTrigger("AoE");
                break;
        }
    }

    private void Update()
    {
        UpdateCoolDown();
    }

    private void UpdateCoolDown()
    {
        if (magicAttacks.Count == 0) return;

        SMagicAttackData attackData = magicAttacks[currentAttackIndex];

        float cooldown = attackData.mCooldown;
        float elapsed = Time.time - lastAttackTime;
        if (elapsed >= cooldown)
        {
            mMagicSelector.onAttackCoolDown(currentAttackIndex, 1f);
        }
        else
        {
            float normalized = (cooldown - elapsed) / cooldown;

            normalized = 1f - normalized;

            mMagicSelector.onAttackCoolDown(currentAttackIndex, normalized);
        }

    }

    // Called by animation event
    private void SpawnMagic()
    {
        SMagicAttackData attackData = magicAttacks[currentAttackIndex];

        if (mCurrentTarget == null) return;

        // Spawn at enemy position
        Vector3 spawnPosition = mCurrentTarget.transform.position;

        // Instantiate the attack prefab at enemy location
        GameObject magicClone = Instantiate(attackData.mAttackPrefab, spawnPosition, Quaternion.identity);

        // If it's AoE, apply AoE logic here
        if (attackData.mIsAoEAttack)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, attackData.mAoERadius);
            foreach (Collider nearbyObj in colliders)
            {
                if (nearbyObj.CompareTag("Enemy"))
                {
                    if (attackData.mIsFreezeAttack && attackData.mSpecialEffectPrefab != null)
                    {
                        GameObject freezeEffect = Instantiate(attackData.mSpecialEffectPrefab, nearbyObj.transform.position, Quaternion.identity);
                        freezeEffect.transform.SetParent(nearbyObj.transform);

                        FreezeEffect freezeScript = freezeEffect.AddComponent<FreezeEffect>();
                        freezeScript.Initialize(nearbyObj.gameObject, attackData.mEffectDuration);
                    }
                    else
                    {
                        Destroy(nearbyObj.gameObject);
                    }
                }
            }
        }
    }
}

