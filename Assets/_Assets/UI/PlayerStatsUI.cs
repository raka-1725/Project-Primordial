using Newtonsoft.Json.Bson;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    Player mPlayer;
    [Header("Health")]
    [SerializeField] private Slider mHealthSlider;
    [SerializeField] private Image mHealthTint;
    [SerializeField] private float mFadeSpeed = 5f;
    private float targetAlpha = 0f;

    [Header("Death")]
    [SerializeField] GameObject mDeathScreen;

    private void Awake()
    {
        mPlayer = FindAnyObjectByType<Player>();
        mDeathScreen.SetActive(false);
        mPlayer.onPlayerDead += PlayerDead;
    }

    private void PlayerDead(Player player)
    {
        Time.timeScale = 0;
        mDeathScreen.SetActive(true);
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        mHealthSlider.value = currentHealth / 100;
        targetAlpha = 1 - (currentHealth / 170);

        //Debug.Log($"Slider{mHealthSlider.value}, sended value {currentHealth}");
    }

    private void Update()
    {
        UpdateFade();
    }

    private void UpdateFade()
    {
        Color c = mHealthTint.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * mFadeSpeed);
        mHealthTint.color = c;
    }
}
