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

    //instead of size delta use scale
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mRectTransform = GetComponent<RectTransform>();
        mNormalScale = mRectTransform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mRectTransform.localScale = mScaleUp;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mRectTransform.localScale = mNormalScale;
    }

    public void onClickReset() 
    {
        mRectTransform.localScale = mNormalScale;
    }
}
