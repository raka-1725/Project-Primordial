using UnityEngine;

public enum AttackAnimationType{ Magic, AoE }

[CreateAssetMenu(fileName = "MagicAttackData", menuName = "Magic/AttackData")]
public class SMagicAttackData : ScriptableObject
{
    [Header("Base Settings")]
    public Sprite mAttackIconSprite;
    public string mAttackName;

    [Header("Attack Settings")]
    public GameObject mAttackPrefab;
    public float mForce = 20f;
    public float mCooldown = 1f;

    [Header("Animation Settings")]
    public AttackAnimationType mAnimationType = AttackAnimationType.Magic;

    [Header("Special Effect Settings")]
    public bool mIsFreezeAttack = false;
    public GameObject mSpecialEffectPrefab;
    public float mEffectDuration = 3f;

    [Header("AoE Attack Settings")]
    public bool mIsAoEAttack = false;
    public float mAoERadius = 3f;
}
