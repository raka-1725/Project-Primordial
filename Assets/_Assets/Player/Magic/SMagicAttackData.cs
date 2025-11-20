using UnityEngine;

[CreateAssetMenu(fileName = "MagicAttackData", menuName = "Magic/AttackData")]
public class SMagicAttackData : ScriptableObject
{
    [Header("Base Settings")]
    public Sprite mAttackIconSprite;
    public string mAttackName;
    public GameObject mAttackPrefab;
    public float mForce = 20f;
    public float mCooldown = 1f;

    [Header("Special Effect Settings")]
    public bool mIsFreezeAttack = false;
    public GameObject mSpecialEffectPrefab;
    public float mEffectDuration = 3f;

    [Header("AoE Attack Settings")]
    public bool mIsAoEAttack = false;
    public float mAoERadius = 3f;
    public float mAoEDamage = 20f;
    public LayerMask mAoETargetLayers; // which layers can be hit by AoE
}
