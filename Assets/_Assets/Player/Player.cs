using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] public float mPlayerHealth { get; private set; } = 100;

    [Header("Player Effects")]
    [SerializeField] private GameObject mDamageEffect;

    PlayerStatsUI mStatsUI;

    public Action<Player> onPlayerDead;

    private void Awake()
    {
        mStatsUI = FindAnyObjectByType<PlayerStatsUI>();
    }
    private void Update()
    {
        if (mPlayerHealth <= 0) 
        {
            onPlayerDead?.Invoke(this);
        }
    }
    public void TakeHealth(float health) 
    {
        mPlayerHealth -= health;
        StartCoroutine(damageEffect());
        mStatsUI.UpdateHealthSlider(mPlayerHealth);
    }

    private IEnumerator damageEffect() 
    {
        GameObject damageEffect = Instantiate(mDamageEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(damageEffect);
    }



}
