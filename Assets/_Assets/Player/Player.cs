using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private float mPlayerHealth = 100;
    [SerializeField] private float mPlayerMana = 50;

    [Header("Player Effects")]
    [SerializeField] private GameObject mDamageEffect;


    public void TakeHealth(float health) 
    {
        mPlayerHealth -= health;
        damageEffect();
    }

    private IEnumerator damageEffect() 
    {
        Instantiate(mDamageEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(mDamageEffect.GetComponent<ParticleSystem>().main.duration);
        Destroy(mDamageEffect);
    }


}
