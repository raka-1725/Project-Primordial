using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeButtonSetter : MonoBehaviour
{
    [SerializeField] Button mNormalB;
    [SerializeField] Button mHCB;
    [SerializeField] Button mTAB;


    [Header("ButtonHighlight")]
    [SerializeField] Image mNormalBImage;
    [SerializeField] Image mHCBImage;
    [SerializeField] Image mTABImage;
    private void Start()
    {
        SetAllImageNotActive();

        mNormalB.Select();
        GameMode.Instance.NormalGameMode();


        mNormalB.onClick.AddListener(modeNormal);
        mHCB.onClick.AddListener(modeHC);
        mTAB.onClick.AddListener(modeTA);
    }

    public void modeNormal()
    {
        mNormalB.Select();
        GameMode.Instance.NormalGameMode();
        SetAllImageNotActive();
        mNormalBImage.gameObject.SetActive(true);
    }

    public void modeHC() 
    {
        mHCB.Select();
        GameMode.Instance.HCGameMode();
        SetAllImageNotActive();
        mHCB.gameObject.SetActive(true);
    }

    public void modeTA() 
    {
        mTAB.Select();
        GameMode.Instance.TAGameMode();
        SetAllImageNotActive();
        mTAB.gameObject.SetActive(true);
    }


    private void SetAllImageNotActive() 
    {
        mNormalBImage.gameObject.SetActive(false);
        mHCB.gameObject.SetActive(false);
        mTAB.gameObject.SetActive(false);
    }

}
