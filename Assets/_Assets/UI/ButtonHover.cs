using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button mButton;

    [Header("Size Settings")]
    [SerializeField] Vector3 mScaleUp = new Vector3 (1.1f,1.1f,1);

    private Vector3 mNormalScale;
    private RectTransform mRectTransform;

    private bool bButtonHovered;
    public event Action<bool> onHoverChanged;
    public bool bIsHovered 
    {
        get => bButtonHovered;
        set 
        {
            bButtonHovered = value;
            onHoverChanged?.Invoke(bButtonHovered);
        }
    }

    //sound
    private AudioManager mAudio;


    //instead of size delta use scale
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mRectTransform = GetComponent<RectTransform>();
        mNormalScale = mRectTransform.localScale;
        mAudio = FindAnyObjectByType<AudioManager>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mRectTransform.localScale = mScaleUp;
        bIsHovered = true;
        mAudio.PlayUISFX();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mRectTransform.localScale = mNormalScale;
        bIsHovered = false;
    }

    public void onClickReset() 
    {
        mRectTransform.localScale = mNormalScale;
    }
}
