using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] public float mPlayerHealth { get; private set; } = 100;
    [SerializeField] private float mPlayerHealthRecoverCoolDown = 10f;
    [SerializeField] private float mHealthRecoveryIndex = 3f;

    private float mHealthRecoverTimer;

    [Header("Player Effects")]
    [SerializeField] private GameObject mDamageEffect;

    PlayerStatsUI mStatsUI;

    public Action<Player> onPlayerDead;
    private bool bIsInRecover;

    private void Awake()
    {
        mStatsUI = FindAnyObjectByType<PlayerStatsUI>();
    }
    public void TakeHealth(float health) 
    {
        mPlayerHealth -= health;
        StartCoroutine(damageEffect());
        mStatsUI.UpdateHealthSlider(mPlayerHealth);
        if (mPlayerHealth <= 0)
        {
            onPlayerDead?.Invoke(this);
        }

        mHealthRecoverTimer = 0;
    }

    private IEnumerator damageEffect() 
    {
        GameObject damageEffect = Instantiate(mDamageEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(damageEffect);
    }

    private void Update()
    {
        HealthRecover();
        //Debug.Log($"Recover timer {mHealthRecoverTimer}, Health recover bool :{bIsInRecover}");

    }

    private void HealthRecover()
    {
        if (mPlayerHealth < 100) 
        {
            mHealthRecoverTimer += Time.deltaTime;
        }
        if (bIsInRecover) 
        {
            mPlayerHealth += Time.deltaTime * mHealthRecoveryIndex;
            mStatsUI.UpdateHealthSlider(mPlayerHealth);
            if (mPlayerHealth >= 100) 
            {
                bIsInRecover = false;
                mHealthRecoverTimer = 0;
            }
            return;
        }
        if (mHealthRecoverTimer >= mPlayerHealthRecoverCoolDown)
        {
            bIsInRecover = true;
            mHealthRecoverTimer = 0;
        }
    }


}
