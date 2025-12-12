using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    Player mPlayer;
    private InputSystem_Actions mInputAction;
    [SerializeField] GameObject mPauseMenu;

    [SerializeField] private bool bPaused;
    AudioManager mAudio;

    private void Awake()
    {
        mInputAction = new InputSystem_Actions();
        mInputAction.Player.Pause.performed += OnPausePressed;
        mInputAction.Player.Enable();
        mPlayer = FindAnyObjectByType<Player>();
        mPauseMenu.SetActive(false);

        mAudio = FindAnyObjectByType<AudioManager>();

        mPlayer.onPlayerDead += DisablePause;
    }

    private void Start()
    {
        mInputAction.Enable();
    }

    private void DisablePause(Player player)
    {
        mInputAction.Disable();
        mAudio.onGameResume.Invoke(mAudio);
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
        mAudio.onGameResume.Invoke(mAudio);
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

    private void Update()
    {
        
    }
}


