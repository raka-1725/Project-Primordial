using System.Collections;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class MainMenuCam : MonoBehaviour
{
    [SerializeField] Canvas optionCanvas;
    [SerializeField] Camera mMainCam;

    [SerializeField] float mRotateDuration = 2f;
    Quaternion originRotation;
    Quaternion targetRotOption = Quaternion.Euler(0, 90, 0);
    Quaternion targetRotSetting = Quaternion.Euler(0, -90, 0);
    Quaternion targetRotControls = Quaternion.Euler(0, -180, 0);

    [SerializeField] GameObject mTransitionPanel;
    private void Start()
    {
        originRotation = mMainCam.transform.rotation;
    }

    public void lookAtOption() 
    {
        StartCoroutine(RotateTo(targetRotOption));  
    }

    public void lookAtSetting()
    {
        StartCoroutine(RotateTo(targetRotSetting));
    }

    public void lookAtControls() 
    {
        StartCoroutine(RotateTo(targetRotControls));
    }

    public void lookAtMain() 
    {
        StartCoroutine(RotateTo(originRotation));
    }

    private IEnumerator RotateTo(Quaternion target) 
    {
        mTransitionPanel.SetActive(true);
        Quaternion startRotation = mMainCam.transform.rotation;
        float time = 0f;
        while (time < mRotateDuration) 
        {
            time += Time.deltaTime;
            float t = time / mRotateDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            mMainCam.transform.rotation = Quaternion.Slerp(startRotation, target, t);
            yield return null;
        }
        mMainCam.transform.rotation = target;

        mTransitionPanel.SetActive(false);
    }
}
