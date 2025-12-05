using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable_UI : MonoBehaviour
{
    [SerializeField] GameObject InteractionUI;
    [SerializeField] TextMeshProUGUI mKeyText_Interaction;
    [SerializeField] TextMeshProUGUI mKeyText_SwitchCam;

    [SerializeField] private InputSystem_Actions mInputAction;
    private void Awake()
    {
        mInputAction.Enable();
        UpdateBindings(mKeyText_Interaction, mInputAction.Player.Interact);
        UpdateBindings(mKeyText_SwitchCam, mInputAction.Player.SwitchCamera);
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

    void UpdateBindings(TextMeshProUGUI text, InputAction InputActions) 
    {
        var action = InputActions;
        string currentBindingInteraction = action.GetBindingDisplayString(bindingIndex : 0);
        text.SetText(currentBindingInteraction);

        Debug.Log(currentBindingInteraction);
    }
}
