using UnityEngine;
using UnityEngine.UI;

public class SkillKeyIconSetter : MonoBehaviour
{
    [SerializeField] Image mKeyIcon;

    [SerializeField] Sprite[] KeySprites;
    [SerializeField] Sprite[] KeySpritesPressed;


    public void SetSkillKeyIcon(int index)
    {
        switch (index)
        {
            case 0: mKeyIcon.sprite = KeySprites[0]; break;
            case 1: mKeyIcon.sprite = KeySprites[1]; break;
            case 2: mKeyIcon.sprite = KeySprites[2]; break;
            case 3: mKeyIcon.sprite = KeySprites[3]; break;
            case 4: mKeyIcon.sprite = KeySprites[4]; break;
            default: mKeyIcon.sprite = null; break;
        }
    }

    public void KeyIconSelected(int index) 
    {
        switch (index)
        {
            case 0: mKeyIcon.sprite = KeySpritesPressed[0]; break;
            case 1: mKeyIcon.sprite = KeySpritesPressed[1]; break;
            case 2: mKeyIcon.sprite = KeySpritesPressed[2]; break;
            case 3: mKeyIcon.sprite = KeySpritesPressed[3]; break;
            case 4: mKeyIcon.sprite = KeySpritesPressed[4]; break;
            default: mKeyIcon.sprite = null; break;
        }
    }
}
