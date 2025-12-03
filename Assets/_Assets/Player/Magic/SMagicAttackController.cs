using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SMagicAttackController : MonoBehaviour
{
    [Header("Magic Attack Settings")]

    [SerializeField] private List<SMagicAttackData> allMagicAttacks; // All possible attacks
    public List<SMagicAttackData> magicAttacks { get; private set; } = new List<SMagicAttackData>(); // Starts empty

    [SerializeField] private Transform magicAttackSpawn;

    private InputSystem_Actions inputActions;
    private Animator animator;
    private MovementController movementController;
    //UI
    private MagicSelector mMagicSelector;

    private int currentAttackIndex = 0;
    private GameObject mCurrentTarget;
    private float[] lastAttackTimes;

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

        lastAttackTimes = new float[magicAttacks.Count];

        for (int i = 0; i < lastAttackTimes.Length; i++) { lastAttackTimes[i] = -10; }
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    public void UnlockAttack(int index)
    {
        if (index < 0 || index >= allMagicAttacks.Count) return;

        SMagicAttackData newAttack = allMagicAttacks[index];
        if (!magicAttacks.Contains(newAttack))
        {
            magicAttacks.Add(newAttack);

            // Resize lastAttackTimes to match magicAttacks.Count
            System.Array.Resize(ref lastAttackTimes, magicAttacks.Count);
            lastAttackTimes[magicAttacks.Count - 1] = -10; // Initialize new slot

            mMagicSelector.NewSkillAccuired(newAttack); // Update UI
            Debug.Log($"Unlocked attack: {newAttack.mAttackName}");

        }
    }

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
        if (Time.time < lastAttackTimes[currentAttackIndex] + attackData.mCooldown) 
        {
            float timeLeft = (lastAttackTimes[currentAttackIndex] + attackData.mCooldown) - Time.time;
            float normalized = 1f - (timeLeft / attackData.mCooldown);


            mMagicSelector.onAttackCoolDown(currentAttackIndex, normalized);
            return;
                
        }
        lastAttackTimes[currentAttackIndex] = Time.time;

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

        for (int i = 0; i < magicAttacks.Count; i++)
        {
            SMagicAttackData attackData = magicAttacks[i];

            float cooldown = attackData.mCooldown;
            float elapsed = Time.time - lastAttackTimes[i];

            float normalized = Mathf.Clamp01(elapsed / cooldown);

            mMagicSelector.onAttackCoolDown(i, normalized);
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
                    //if (attackData.mIsFreezeAttack && attackData.mSpecialEffectPrefab != null)
                    //{
                    //    GameObject freezeEffect = Instantiate(attackData.mSpecialEffectPrefab, nearbyObj.transform.position, Quaternion.identity);
                    //    freezeEffect.transform.SetParent(nearbyObj.transform);

                    //    FreezeEffect freezeScript = freezeEffect.AddComponent<FreezeEffect>();
                    //    freezeScript.Initialize(nearbyObj.gameObject, attackData.mEffectDuration);
                    //}
                    //else
                    {
                        Destroy(nearbyObj.gameObject);
                    }
                }
            }
        }
    }
}

