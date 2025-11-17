using UnityEngine;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Sprite mIcon;
    [SerializeField] private string mSkillName;
   public void UpdateIcon(Sprite icon, string name) 
   {
        mIcon = icon; 
        mSkillName = name;
   }
}
