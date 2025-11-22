using UnityEngine;

public class SProjectileLogic : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;

    public bool isFreezeAttack = false;
    public GameObject mSpecialEffectPrefab;
    public float mEffectDuration = 3f;
    public bool isAoEAttack = false;
    public float aoeRadius = 3f;

    public void Initialize(SMagicAttackData attackData)
    {
        isFreezeAttack = attackData.mIsFreezeAttack;
        mSpecialEffectPrefab = attackData.mSpecialEffectPrefab;
        mEffectDuration = attackData.mEffectDuration;
        isAoEAttack = attackData.mIsAoEAttack;
        aoeRadius = attackData.mAoERadius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player")) return;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        float radius = isAoEAttack ? aoeRadius : 1f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObj in colliders)
        {
            if (nearbyObj.CompareTag("Enemy"))
            {
                if (isFreezeAttack && mSpecialEffectPrefab != null)
                {
                    GameObject freezeEffect = Instantiate(mSpecialEffectPrefab, nearbyObj.transform.position, Quaternion.identity);
                    freezeEffect.transform.SetParent(nearbyObj.transform);

                    FreezeEffect freezeScript = freezeEffect.AddComponent<FreezeEffect>();
                    freezeScript.Initialize(nearbyObj.gameObject, mEffectDuration);
                }
                else
                {
                    Destroy(nearbyObj.gameObject);
                    Debug.Log($"Enemy destroyed: {nearbyObj.name}");
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        float radius = isAoEAttack ? aoeRadius : 1f;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}