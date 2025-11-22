using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    //Can only be one, top takes priority
    [SerializeField] private bool IsKey;
    [SerializeField] private bool IsMedkit;
    //public bool IsSpeedBoost;
    //public bool Is
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Item in range");
        }
    }
    public void Activate(Interactions player)
    {
        if (IsKey)
        {
            KeyPickup(player);
        }
        if (IsMedkit)
        {
            HPPickup(player);
        }
        else
        {
            return;
        }
    }
    private void KeyPickup(Interactions player)
    {
        player.mKeys++;
        Debug.Log($"You have {player.mKeys} keys");
        Pickedup();
    }
    private void HPPickup(Interactions player)
    {
        // player heals ex: player.Heal()
        Debug.Log($"Heal HP");
        Pickedup();
    }
    private void Pickedup()
    {
        Destroy(gameObject);
    }
}
