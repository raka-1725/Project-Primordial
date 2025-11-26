using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject mObject = null; // Door or anything to activate
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            Activate();
        }
    }

    private void Activate()
    {
        Debug.Log("Pressure plate activated!");
        if (mObject == null)
            return;
        Door door = mObject.GetComponent<Door>();
        if (door != null)
        {
            door.ActivateFromLever(); // your custom method
        }
    }
}
