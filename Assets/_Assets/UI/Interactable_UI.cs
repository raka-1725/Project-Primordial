using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable_UI : MonoBehaviour
{
    [SerializeField] GameObject InteractionUI;
    [SerializeField] TextMeshProUGUI mKeyText_Interaction;
    [SerializeField] TextMeshProUGUI mKeyText_SwitchCam;
    [SerializeField] private InputActionAsset mInputActionAsset;
    private InputSystem_Actions mInputAction;
    private void Awake()
    {
        mInputAction = new InputSystem_Actions();
        LoadBindingOverrides();
        UpdateBindings(mKeyText_Interaction, mInputAction.Player.Interact);
        UpdateBindings(mKeyText_SwitchCam, mInputAction.Player.SwitchCamera);
        DisableUI();
    }

    void OnEnable() { mInputAction.Player.Enable(); }
    void OnDisable() { mInputAction.Player.Disable(); }

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

        Debug.Log($"current binding : {currentBindingInteraction}");
    }
    private void LoadBindingOverrides()
    {
        if (PlayerPrefs.HasKey("InputOverrides"))
        {
            string json = PlayerPrefs.GetString("InputOverrides");
            var playerMap = mInputAction.asset.FindActionMap("Player");
            playerMap.LoadBindingOverridesFromJson(json);
            Debug.Log("Loaded binding overrides");
        }
    }

}
