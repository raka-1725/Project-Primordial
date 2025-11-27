using System.Collections;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class MainMenuCam : MonoBehaviour
{
    [SerializeField] Canvas optionCanvas;
    [SerializeField] Camera mMainCam;

    [SerializeField] float mRotateDuration = 2f;
    Quaternion originRotation;
    Quaternion targetRot = Quaternion.Euler(0, 90, 0);
    private void Start()
    {
        originRotation = mMainCam.transform.rotation;
    }

    public void lookAtOption() 
    {
        StartCoroutine(RotateTo(targetRot));  
    }

    public void lookAtMain() 
    {
        StartCoroutine(RotateTo(originRotation));
    }

    private IEnumerator RotateTo(Quaternion target) 
    {
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
    }
}
