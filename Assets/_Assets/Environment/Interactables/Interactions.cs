using UnityEngine;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{
    private InputSystem_Actions mInputAction;

    private Collider mInteractableInRange;

    public int mKeys;

    

    void Awake()
        {
            mInputAction = new InputSystem_Actions();
            LoadBindingOverrides();
            mInputAction.Player.Interact.performed += TryInteraction;
        }
    private void OnEnable() => mInputAction.Enable();
    private void OnDisable() => mInputAction.Disable();

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
    private void TryInteraction(InputAction.CallbackContext context)
    {
         if (mInteractableInRange != null)
        {
            IInteractable interactable = mInteractableInRange.GetComponent<IInteractable>();
                if (interactable != null)
                    {
                        interactable.Activate(this);
                    }
            
            /*Lever lever = mInteractableInRange.GetComponent<Lever>();
            if (lever != null)
            {
                lever.Activate();
            }
            Door door = mInteractableInRange.GetComponent<Door>();
            if (door != null)
            {
                door.Activate();
            }
            Key key = mInteractableInRange.GetComponent<Key>();
            if (key != null)
            {
                mKeys++;
                key.OnPickup();
                 Debug.Log($"You have {mKeys} keys");
            }*/
        }
    }
     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("Interactable In Range");
            mInteractableInRange = other;

            //UI
            Interactable_UI InteractableUI = FindAnyObjectByType<Interactable_UI>();
            InteractableUI.EnableUI();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == mInteractableInRange)
        {
            mInteractableInRange = null;
            Interactable_UI InteractableUI = FindAnyObjectByType<Interactable_UI>();
            InteractableUI.DisableUI();
        }
    }
}
