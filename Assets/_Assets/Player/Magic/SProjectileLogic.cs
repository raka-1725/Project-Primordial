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

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with Player
        if (collision.transform.CompareTag("Player")) return;

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Determine radius
        float radius = isAoEAttack ? aoeRadius : 1f;

        // Find all objects in radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObj in colliders)
        {
            if (nearbyObj.CompareTag("Enemy"))
            {
                if (isFreezeAttack && mSpecialEffectPrefab != null)
                {
                    // Freeze logic
                    GameObject freezeEffect = Instantiate(mSpecialEffectPrefab, nearbyObj.transform.position, Quaternion.identity);
                    freezeEffect.transform.SetParent(nearbyObj.transform);

                    FreezeEffect freezeScript = freezeEffect.AddComponent<FreezeEffect>();
                    freezeScript.Initialize(nearbyObj.gameObject, mEffectDuration);
                }
                else
                {
                    // Normal attack: destroy enemy
                    Destroy(nearbyObj.gameObject);
                    Debug.Log($"Enemy destroyed: {nearbyObj.name}");
                }
            }
        }

        // Destroy projectile after collision processing
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        float radius = isAoEAttack ? aoeRadius : 1f;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}