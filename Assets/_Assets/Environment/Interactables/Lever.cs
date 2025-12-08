using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    private bool Flipped = false;
    [SerializeField] private GameObject mObject = null;

    public void Activate(Interactions player)
    {
        Flipped = !Flipped; // toggles between true/false
        Debug.Log("Lever flipped: " + Flipped);
        OpenDoor(player);
    }
    private void OpenDoor(Interactions player)
    {
        Door door = mObject.GetComponent<Door>();
            if (door != null)
            {
                door.ActivateFromLever();
            }
    }
    public string GetInteractText()
    {
        return "";
    }
}
