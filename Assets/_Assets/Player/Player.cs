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

    private void Awake()
    {
        mStatsUI = FindAnyObjectByType<PlayerStatsUI>();
    }

    public void TakeHealth(float health) 
    {
        mPlayerHealth -= health;
        damageEffect();
        mStatsUI.UpdateHealthSlider(mPlayerHealth);
        Debug.Log($"Player damage, Take helth{health}, current health{mPlayerHealth}");
    }

    private IEnumerator damageEffect() 
    {
        Instantiate(mDamageEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(mDamageEffect);
    }


}
