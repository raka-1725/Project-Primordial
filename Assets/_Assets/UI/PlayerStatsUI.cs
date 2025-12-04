using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    GameManager mGameManager;
    Player mPlayer;
    [Header("Health")]
    [SerializeField] private Slider mHealthSlider;


    [SerializeField] private Image mHealthTint;
    [SerializeField] private float mFadeSpeed = 5f;
    private float targetAlpha = 0f;



    [Header("Death")]
    [SerializeField] GameObject mDeathScreen;
    [SerializeField] TextMeshProUGUI mDeathText;
    [SerializeField] string mDeathMSG;

    [Header("Win")]
    [SerializeField] GameObject mWinScreen;
    [SerializeField] TextMeshProUGUI mWinText;
    [SerializeField] string mWinMSG;

    [Header("Typo effect settings")]
    [SerializeField] float mCharacterPerSec = 5f;

    private void Awake()
    {
        mGameManager = FindAnyObjectByType<GameManager>();
        mPlayer = FindAnyObjectByType<Player>();
        mDeathScreen.SetActive(false);
        mWinScreen.SetActive(false);
         
        mPlayer.onPlayerDead += PlayerDead;
        mGameManager.onPlayerGoal += PlayerWin;
    }
    private void OnDestroy()
    {
        mPlayer.onPlayerDead -= PlayerDead;
        mGameManager.onPlayerGoal -= PlayerWin;
    }

    private void PlayerWin(GameManager manager)
    {
        Time.timeScale = 0;
        mWinScreen.SetActive(true);
        StartCoroutine(PlayerDeadSequence());
    }

    private void PlayerDead(Player player)
    {
        Time.timeScale = 0;
        mDeathScreen.SetActive(true);

        StartCoroutine(PlayerWinSequence());
    }
    private IEnumerator PlayerDeadSequence() 
    {
        yield return StartCoroutine(ScreenFadeIn(1.2f, mWinScreen));
        yield return StartCoroutine(TypeText(mWinMSG, mWinText));
    }

    private IEnumerator PlayerWinSequence() 
    {
        yield return StartCoroutine(ScreenFadeIn(1.2f, mDeathScreen));
        yield return StartCoroutine(TypeText(mDeathMSG, mDeathText));
    }
    private IEnumerator ScreenFadeIn(float duration, GameObject screen) 
    {
        Image BGImgage = screen.GetComponentInChildren<Image>();
        float elapsed = 0f;
        Color C = BGImgage.color;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            C.a = Mathf.Clamp01(elapsed / duration);
            BGImgage.color = C;
            yield return null;
        }
        yield return new WaitForSeconds(duration + 0.5f);
    }

    private IEnumerator TypeText(string line, TextMeshProUGUI textComponent) 
    {
        textComponent.text = "";
        foreach (char c in line) 
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(1f / mCharacterPerSec);
            //typo sound???
        }
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
