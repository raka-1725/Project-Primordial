using UnityEngine;

public class FreezeEffect : MonoBehaviour
{

    private GameObject enemy;
    private float duration;

    public void Initialize(GameObject targetEnemy, float freezeDuration)
    {
        enemy = targetEnemy;
        duration = freezeDuration;

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.Freeze(duration);
            Debug.Log($"Enemy Frozed:{enemy.name}");
        }

        Invoke(nameof(UnfreezeEnemy), duration);
    }

    private void UnfreezeEnemy()
    {
        Destroy(gameObject); // Remove freeze cube
    }

}
