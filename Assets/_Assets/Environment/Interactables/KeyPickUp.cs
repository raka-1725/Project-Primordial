using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log;
        }
    }
}
