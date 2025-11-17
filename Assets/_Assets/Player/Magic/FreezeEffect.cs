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
            enemyScript.Freeze();
        }

        Invoke(nameof(UnfreezeEnemy), duration);
    }

    private void UnfreezeEnemy()
    {
        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.UnFreeze();
            }
        }

        Destroy(gameObject); // Remove freeze cube
    }

}
