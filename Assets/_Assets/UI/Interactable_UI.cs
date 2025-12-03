using UnityEngine;

public class Interactable_UI : MonoBehaviour
{
    [SerializeField] GameObject InteractionUI;
    private void Awake()
    {
        DisableUI();
    }
    public void EnableUI() 
    {
        InteractionUI.SetActive(true);
    }

    public void DisableUI() 
    {
        InteractionUI.SetActive(false);
    }
}
