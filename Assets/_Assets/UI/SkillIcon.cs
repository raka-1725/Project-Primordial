using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Sprite mIconSprite;
    [SerializeField] private Image mIconImageComponent;
    [SerializeField] private string mSkillName;
   public void UpdateIcon(Sprite icon, string name) 
   {
        mIconSprite = icon;
        mSkillName = name;
        mIconImageComponent.sprite = mIconSprite;
   }

    public void ChangeScale(float width, float height) 
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
        mIconImageComponent.rectTransform.sizeDelta = new Vector2(width,height);
    }
    
}
