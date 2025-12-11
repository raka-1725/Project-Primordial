using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrintControls : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mKeyText_Interaction;
    [SerializeField] TextMeshProUGUI mKeyText_SwitchCam;
    [SerializeField] TextMeshProUGUI mKeyText_Jump;
    [SerializeField] TextMeshProUGUI mKeyText_Attack;

    private InputSystem_Actions mInputAction;

    

    private void Start()
    {
        mInputAction = new InputSystem_Actions();
    }

    public void RePrint() 
    {
        LoadBindingOverrides();
        UpdateBindings(mKeyText_Interaction, mInputAction.Player.Interact);
        UpdateBindings(mKeyText_SwitchCam, mInputAction.Player.SwitchCamera);
        UpdateBindings(mKeyText_Jump, mInputAction.Player.Jump);
        UpdateBindings(mKeyText_Attack, mInputAction.Player.Attack);
    }


    void UpdateBindings(TextMeshProUGUI text, InputAction InputActions)
    {
        var action = InputActions;
        string currentBindingInteraction = action.GetBindingDisplayString(bindingIndex: 0);
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
