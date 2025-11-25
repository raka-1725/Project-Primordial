using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Sprite mIconSprite;
    [SerializeField] private Image mIconImageComponent;
    
    [SerializeField] private string mSkillName;

    [SerializeField] private GameObject mDisabledTint;
    [SerializeField] private Slider mCoolDownSlider;
    [SerializeField] private TextMeshProUGUI mSkillNameText;


   public void UpdateIcon(SMagicAttackData magicAttackData) 
   {
        mIconSprite = magicAttackData.mAttackIconSprite;
        mSkillName = magicAttackData.mAttackName;
        mIconImageComponent.sprite = mIconSprite;
        mSkillNameText.SetText(mSkillName);


   }

    public void ChangeScale(float scaleIndex) 
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(scaleIndex, scaleIndex, scaleIndex);
    }

    public void OnCoolDown(float value) 
    {
        mCoolDownSlider.value = value;
    }
}
