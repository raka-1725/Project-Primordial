using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpeningSequence : MonoBehaviour
{
    [SerializeField] Image mIconImage;
    [SerializeField] Image mOpeningBG;

    [SerializeField] GameObject mIcon;
    [SerializeField] GameObject mBG;
    [Header("Settings")]
    [SerializeField] float mFadeInDuration;
    [SerializeField] float mShowDuration;
    [SerializeField] float mFadeOutDuration;
    [SerializeField] float mDiffDuration;
    private void Awake()
    {
        mIcon.SetActive(true);
        mBG.SetActive(true);
        SetImageAlpha(mIconImage, 0);
        SetImageAlpha(mOpeningBG, 1);

        StartCoroutine(OpeningSequenceCoroutine());
    }

    private void DisableGameObjects()
    {
        mIcon.SetActive(false);
        mBG.SetActive(false);
    }

    private void SetImageAlpha(Image image, float aplha)
    {
        Color colorA = image.color;
        colorA.a = aplha;
        image.color = colorA;
    }

    private IEnumerator OpeningSequenceCoroutine() 
    {
        SetImageAlpha(mIconImage, 0);
        SetImageAlpha(mOpeningBG, 1);

        float time = 0;

        while (time < mFadeInDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Clamp01(time / mFadeInDuration);
            SetImageAlpha(mIconImage, alpha);
            yield return null;
        }
        
        yield return new WaitForSeconds(mShowDuration);

        time = 0;
        while (time < mFadeOutDuration) 
        {
            time += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(time / mFadeOutDuration);
            SetImageAlpha(mIconImage, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(mDiffDuration);


        //BG fadeout
        time = 0;
        while (time < mFadeOutDuration)
        {
            time += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(time / mFadeOutDuration);
            SetImageAlpha(mOpeningBG, alpha);
            yield return null;
        }

        DisableGameObjects();
    }
}
