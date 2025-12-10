using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeButtonSetter : MonoBehaviour
{
    [SerializeField] Button mNormalB;
    [SerializeField] Button mHCB;
    [SerializeField] Button mTAB;

    private void Start()
    {
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
    }

    public void modeHC() 
    {
        mHCB.Select();
        GameMode.Instance.HCGameMode();
    }

    public void modeTA() 
    {
        mTAB.Select();
        GameMode.Instance.TAGameMode();
    }


}
