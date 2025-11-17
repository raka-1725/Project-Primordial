using UnityEngine;

[CreateAssetMenu(fileName = "MagicAttackData", menuName = "Magic/AttackData")]
public class SMagicAttackData : ScriptableObject
{
    public string mAttackName;
    public GameObject mAttackPrefab;
    public float mForce = 20f;
    public float mCooldown = 1f;

    [Header("Special Effect Settings")]
    public bool mIsFreezeAttack = false;
    public GameObject mSpecialEffectPrefab;
    public float mEffectDuration = 3f;
}
