using System;
using UnityEngine;
using UnityEngine.AI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource mMusic;
    [SerializeField] AudioSource mSFX;
    [SerializeField] AudioSource mSFX_Magic;
    [SerializeField] AudioSource mBG;

    [Header("AudioClip")]
    public AudioClip background;
    public AudioClip takeDamage;
    public AudioClip fireMagic;
    public AudioClip iceMagic;
    public AudioClip AoEMagic;
    public AudioClip UIClick;
    public AudioClip LowHealth;
    public AudioClip HealthRecover;
    public AudioClip TypoSound;
    public AudioClip DeathSound;
    public AudioClip WinSound;

    public Action<AudioManager> onGamePause;
    public Action<AudioManager> onGameResume;

    public Action<AudioManager> onVolumeChanged;


    private void Awake()
    {
        onGamePause += onPause;
        onGameResume += onResume;
    }
    private void Update()
    {
        updateVolume();
    }
    private void updateVolume()
    {
        mMusic.volume = PlayerPrefs.GetFloat("musicVolume");
        mBG.volume = PlayerPrefs.GetFloat("musicVolume");
        mSFX.volume = PlayerPrefs.GetFloat("SFXVolume");
        mSFX_Magic.volume = PlayerPrefs.GetFloat("SFXVolume");
    }

    private void onResume(AudioManager manager)
    {
        mBG.Play();

    }

    private void onPause(AudioManager manager)
    {
        mBG.Pause();
    }

    private void Start()
    {
        mBG.clip = background;
        mBG.loop = true;
        mBG.Play();
    }

    public void PlayTakeDamage() 
    {
        mSFX.clip = takeDamage;
        mSFX.Play();
    }

    public void PlayFireMagic() 
    {
        mSFX_Magic.clip = fireMagic;
        mSFX_Magic.Play();
    }

    public void PlayIceMagic() 
    {
        mSFX_Magic.clip = iceMagic;
        mSFX_Magic.Play();
    }

    public void PlayAoEMagic() 
    {
        mSFX_Magic.clip = AoEMagic;
        mSFX_Magic.Play();
    }

    public void PlayUISFX() 
    {
        mSFX.clip = UIClick;
        mSFX.Play();
    }

    public void PlayLowHealth() 
    {
        mSFX.clip = LowHealth;
        mSFX.Play();
    }

    public void PlayerRecoverHealth() 
    {
        mSFX.clip = HealthRecover;
        mSFX.Play();
    }

    public void PlayTypo() 
    {
        mSFX.clip = TypoSound;
        mSFX.Play();
    }

    public void PlayWinSound() 
    {
        mMusic.volume = mMusic.volume/2;
        mSFX.clip = WinSound;
        mSFX.Play();
    }

    public void PlayLoseSound() 
    {
        mMusic.volume = mMusic.volume / 2;
        mSFX.clip = DeathSound;
        mSFX.Play();
    }
}
