using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private float mPlayerHealth = 100;
    [SerializeField] private float mPlayerMana = 50;




    public void TakeHealth(float health) 
    {
        mPlayerHealth -= health;
    }
}
