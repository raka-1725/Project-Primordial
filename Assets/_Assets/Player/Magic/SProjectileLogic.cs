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

    private bool bHasCollided = false;

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
        if(bHasCollided) return;
        bHasCollided = true;
        if (collision.transform.CompareTag("Player")) { bHasCollided = false; return; }

        if (explosionEffect != null) 
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            bHasCollided = false;
        }

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
                    FreezeEffect freezeScript = freezeEffect.GetComponent<FreezeEffect>();
                    if (freezeScript == null)
                        freezeScript = freezeEffect.AddComponent<FreezeEffect>();

                    freezeScript.Initialize(nearbyObj.gameObject, mEffectDuration);
                    bHasCollided = false;
                }
                else
                {
                    Destroy(nearbyObj.gameObject);
                    Debug.Log($"Enemy destroyed: {nearbyObj.name}");
                    bHasCollided = false;
                }
            }
        }
        if (collision.transform.gameObject) //add detection system and timer to destroy
        {
            Destroy(gameObject);
            bHasCollided = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        float radius = isAoEAttack ? aoeRadius : 1f;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}