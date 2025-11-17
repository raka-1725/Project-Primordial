using UnityEngine;
using UnityEngine.SceneManagement;

public class SFireballExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius = 1.0f;

    public bool isFreezeAttack = false;
    public GameObject mSpecialEffectPrefab;
    public float mEffectDuration = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with Player
        if (collision.transform.CompareTag("Player")) return;

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Find all objects in radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObj in colliders)
        {
            // Destroy only enemies
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
                    // Fireball logic: destroy enemy
                    Destroy(nearbyObj.gameObject);
                    Debug.Log($"Enemy destroyed: {nearbyObj.name}");
                }


            }
        }

        // Destroy projectile after explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
