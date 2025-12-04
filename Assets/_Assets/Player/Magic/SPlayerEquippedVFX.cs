using UnityEngine;

public class SPlayerEquippedVFX : MonoBehaviour
{
    public Transform handVFXAttachPoint;

    private GameObject currentVFX;

    public void SetEquippedVFX(SMagicAttackData attackData)
    {
        Debug.Log("SetEquippedVFX CALLED");

        if (currentVFX != null)
            Destroy(currentVFX);

        if (attackData == null)
        {
            Debug.Log("No attack data!");
            return;
        }

        if (attackData.mEquippedHandVFX == null)
        {
            Debug.Log("No equipped VFX assigned in the ScriptableObject!");
            return;
        }

        currentVFX = Instantiate(attackData.mEquippedHandVFX, handVFXAttachPoint);
        currentVFX.transform.localPosition = Vector3.zero;
        // Scale down the VFX to fit hand
        currentVFX.transform.localScale = new Vector3(0.000001f, 0.000001f, 0.000001f);

        Debug.Log($"VFX Spawned at position: {currentVFX.transform.position}");
        Debug.Log($"Hand attach point position: {handVFXAttachPoint.position}");
        Debug.Log($"VFX parent: {currentVFX.transform.parent.name}");
    }


}
