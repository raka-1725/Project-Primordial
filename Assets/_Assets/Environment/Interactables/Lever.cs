using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool Flipped = false;
    [SerializeField] private GameObject mObject = null;

    public void Activate()
    {
        Flipped = !Flipped; // toggles between true/false
        Debug.Log("Lever flipped: " + Flipped);
        OpenDoor();
    }
    private void OpenDoor()
    {
        Door door = mObject.GetComponent<Door>();
            if (door != null)
            {
                door.Activate();
            }
    }
}
