using UnityEngine;

public class SFloppyDiskPickup : MonoBehaviour
{
    [SerializeField] private int attackIndexToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SMagicAttackController controller = other.GetComponent<SMagicAttackController>();
            if (controller != null)
            {
                controller.UnlockAttack(attackIndexToUnlock);
                Destroy(gameObject);
            }
        }
    }

}
