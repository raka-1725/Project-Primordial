using UnityEngine;
using UnityEngine.SceneManagement;

public class SFireballExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius = 1.0f;

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
                Destroy(nearbyObj.gameObject);
                Debug.Log($"Enemy destroyed: {nearbyObj.name}");
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
