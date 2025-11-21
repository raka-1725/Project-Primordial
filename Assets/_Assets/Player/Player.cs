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

    }
}
