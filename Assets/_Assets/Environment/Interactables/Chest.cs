using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private bool IsKey;
    [SerializeField] private bool mIsOpen = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Chest in range");
        }
    }
    public void Activate(Interactions player)
    {
        if (mIsOpen = true)
            return;
        if (IsKey)
        {
            KeyPickup(player);
        }
    }
    private void KeyPickup(Interactions player)
    {
        player.mKeys++;
        Debug.Log($"You have {player.mKeys} keys");
        Pickedup();
    }
    private void Pickedup()
    {
        mIsOpen = true;
    }
}
