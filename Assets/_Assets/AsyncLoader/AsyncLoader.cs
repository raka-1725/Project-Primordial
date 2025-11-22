using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject mLoadingScreen;

    [Header("Slider")]
    [SerializeField] private Slider mLoadingSlider;

    public void LoadLevel(string levelToLoad)
    {
        mLoadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            mLoadingSlider.value = progressValue;
            yield return null;
        }
    }
}
