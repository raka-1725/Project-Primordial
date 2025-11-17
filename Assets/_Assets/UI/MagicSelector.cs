using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MagicSelector : MonoBehaviour
{
    SMagicAttackController mMagicAttackController;
    List<SMagicAttackData> mMagicAttackData;

    private int mMagicAttackIndex;

    [Header("UI")]

    [SerializeField] private GameObject mSkillIconPrefab;


    [SerializeField] private TextMeshProUGUI mSelectedskillText;

    [SerializeField] private GameObject mSelectedSkill;

    [SerializeField] private Transform mSkillSliderParent;

    [SerializeField] private List<Sprite> mSkillIconSprites; //change this once there is sprite variable in the scriptable object
    private int currentSelectedIndex;

    public void NewSkillAccuired(SMagicAttackData magicdata) 
    {
        GameObject newSkillIconObj = Instantiate(mSkillIconPrefab, mSkillSliderParent);
        newSkillIconObj.GetComponent<SkillIcon>().UpdateIcon(mSkillIconSprites[0], magicdata.mAttackName); 
    }

    public void UpdateAttackInventoryUI(int selectedIndex, List<SMagicAttackData> magicAttackDataList) 
    {



    }

}
