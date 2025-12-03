using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class MagicSelector : MonoBehaviour
{
    SMagicAttackController mMagicAttackController;
    [SerializeField] List<SMagicAttackData> mMagicAttackData;

    private int mMagicAttackIndex;

    [Header("UI")]

    [SerializeField] private GameObject mSkillIconPrefab;


    [SerializeField] private TextMeshProUGUI mSelectedskillText;

    [SerializeField] private GameObject mSelectedSkill;

    [SerializeField] private Transform mSkillSliderParent;

    [SerializeField] private float ScaleUpScale = 1.2f;

    [SerializeField] private float DefaultScale = 1;

    [SerializeField] private List<GameObject> mSkillUIList;
    [SerializeField] private List<Sprite> mSkillIconSprites; //change this once there is sprite variable in the scriptable object
    private int currentAttackIndex;

    private void Start()
    {
        mMagicAttackController = FindAnyObjectByType<SMagicAttackController>();
        mMagicAttackData = mMagicAttackController.magicAttacks;

        SetUpUISkillList(mMagicAttackData);
    }

    private void SetUpUISkillList(List<SMagicAttackData> magicAttackDataList) 
    {
        foreach (SMagicAttackData magicAttackData in magicAttackDataList) 
        {
            GameObject SkillIcon = Instantiate(mSkillIconPrefab, mSkillSliderParent);
            SkillIcon.GetComponent<SkillIcon>().UpdateIcon(magicAttackData);
            mSkillUIList.Add(SkillIcon);
        }
    }

    public void NewSkillAccuired(SMagicAttackData magicAttackData) 
    {
        GameObject newSkillIconObj = Instantiate(mSkillIconPrefab, mSkillSliderParent);
        newSkillIconObj.GetComponent<SkillIcon>().UpdateIcon(magicAttackData);
        mSkillUIList.Add(newSkillIconObj);
    }

    public void UpdateAttackInventoryUI(int selectedIndex, List<SMagicAttackData> magicAttackDataList) 
    {
        // Clear old UI icons
        foreach (GameObject skillIconObj in mSkillUIList)
        {
            Destroy(skillIconObj);
        }
        mSkillUIList.Clear();

        // Create new icons for each unlocked attack
        foreach (SMagicAttackData magicAttackData in magicAttackDataList)
        {
            GameObject newSkillIconObj = Instantiate(mSkillIconPrefab, mSkillSliderParent);
            newSkillIconObj.GetComponent<SkillIcon>().UpdateIcon(magicAttackData);
            mSkillUIList.Add(newSkillIconObj);
        }

        // Highlight the selected skill if valid
        if (selectedIndex >= 0 && selectedIndex < mSkillUIList.Count)
        {
            SelectAbility(selectedIndex);
        }

    }


    public void SelectAbility(int index) 
    {
        foreach (GameObject SkillIcons in mSkillUIList) 
        {
            SkillIcon skillIcon = SkillIcons.GetComponent<SkillIcon>();
            skillIcon.ChangeScale(DefaultScale);
        }
        mSkillUIList[index].GetComponent<SkillIcon>().ChangeScale(ScaleUpScale);
    }

    void Awake()
    {
    }

    public void UpdateCycleAttackIndex(int index) 
    {
        currentAttackIndex = index;
        SelectAbility(currentAttackIndex);
    }


    public void onAttackCoolDown(int listindex, float value) 
    {
        mSkillUIList[listindex].GetComponent<SkillIcon>().OnCoolDown(value);
    }
}
