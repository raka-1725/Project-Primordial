using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private InputSystem_Actions mInputAction;
    [SerializeField] GameObject mPauseMenu;

    [SerializeField] private bool bPaused;

    private void Awake()
    {
        mInputAction = new InputSystem_Actions();
        mInputAction.Player.Pause.performed += OnPausePressed;
        mInputAction.Player.Enable();

        mPauseMenu.SetActive(false);
    }
    private void OnEnable() => mInputAction.Enable();
    private void OnDisable()
    {
        mInputAction.Disable();
        Time.timeScale = 1.0f;
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        bPaused = !bPaused;
        mPauseMenu.SetActive(bPaused);
        Paused();
    }

    public void Paused()
    {
        Time.timeScale = bPaused ? 0f : 1f;
    }

    public void Resume()
    {
        bPaused = false;
        mPauseMenu.SetActive(bPaused);
        Time.timeScale = 1.0f;
    }

    
}


