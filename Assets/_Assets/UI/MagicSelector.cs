using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicSelector : MonoBehaviour
{
    SMagicAttackController mMagicAttackController;
    List<SMagicAttackData> mMagicAttackData;

    private int mMagicAttackIndex;

    [Header("UI")]

    [SerializeField] private Sprite mSelectedSkillIcon;
    [SerializeField] private TextMeshProUGUI mSelectedskillText;

    [SerializeField] private Sprite mPreviousSkillIcon;
    [SerializeField] private Sprite m2PreviousSkillIcon;
    [SerializeField] private Sprite mNextSkillIcon;
    [SerializeField] private Sprite m2NextSkillIcon;



    public void UpdateAttackInventoryUI(int selectedMagicIndex) 
    {
        
        
    }

}
