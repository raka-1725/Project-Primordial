using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartSequence : MonoBehaviour
{
    [SerializeField] private float mCharacterPerSec = 10;
    [SerializeField] private GameObject mOpenigSequence;
    [SerializeField] private Image mOSBGImage;
    [SerializeField] private TextMeshProUGUI mText;
    [Header("Settings")]
    [SerializeField] float mFadeOutDuration;

    [Header("MSG")]
    [SerializeField] string mCutSceneMSG;



    private void Awake()
    {
        Time.timeScale = 0;
        StartCoroutine(GameStartSequenceCoroutine());
    }

    private IEnumerator GameStartSequenceCoroutine() 
    {
        yield return TypeText(mCutSceneMSG, mText);
        mText.gameObject.SetActive(false);
        yield return ScreenFadeOut(mFadeOutDuration, mOSBGImage);

        mOpenigSequence.SetActive(false);
        Time.timeScale = 1;
    }

    private IEnumerator ScreenFadeOut(float duration, Image BGImage)
    {
        float elapsed = 0f;
        Color C = BGImage.color;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            C.a = Mathf.Lerp(C.a, 0, t);
            BGImage.color = C;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.1f);
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
}
