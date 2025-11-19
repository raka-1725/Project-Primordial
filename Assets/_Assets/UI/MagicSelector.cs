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

    [SerializeField] private float ScaleUpWidth = 90;
    [SerializeField] private float ScaleUpHeight = 90;

    [SerializeField] private float DefaultWidth = 50;
    [SerializeField] private float DefaultHeight = 50;

    [SerializeField] private List<GameObject> mSkillUIList;
    [SerializeField] private List<Sprite> mSkillIconSprites; //change this once there is sprite variable in the scriptable object
    private int currentSelectedIndex;
    private InputSystem_Actions inputActions;

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
            //SkillIcon.GetComponent<SkillIcon>().UpdateIcon(magicAttackData.mSkillIcon, magicAttackData.mAttackName); //waiting for Scriptable object to be updated
            mSkillUIList.Add(SkillIcon);
        }
    }

    public void NewSkillAccuired(SMagicAttackData magicAttackData) 
    {
        GameObject newSkillIconObj = Instantiate(mSkillIconPrefab, mSkillSliderParent);
        //newSkillIconObj.GetComponent<SkillIcon>().UpdateIcon(magicAttackData.mSkillIcon, magicAttackData.mAttackName);
        mSkillUIList.Add(newSkillIconObj);
    }

    public void UpdateAttackInventoryUI(int selectedIndex, List<SMagicAttackData> magicAttackDataList) 
    {



    }


    public void SelectAbility(int index) 
    {
        foreach (GameObject SkillIcons in mSkillUIList) 
        {
            SkillIcon skillIcon = SkillIcons.GetComponent<SkillIcon>();
            skillIcon.ChangeScale(DefaultWidth, DefaultHeight);
        }
        mSkillUIList[index].GetComponent<SkillIcon>().ChangeScale(ScaleUpWidth, ScaleUpHeight);
    }


    [SerializeField] private int currentAttackIndex;

    void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.SwitchAttackScroll.performed += ctx =>
        {
            Vector2 scrollValue = ctx.ReadValue<Vector2>();
            if (scrollValue.y > 0) CycleAttackScroll(1);
            else if (scrollValue.y < 0) CycleAttackScroll(-1);
        };

        inputActions.Player.SwitchAttackKey.performed += ctx =>
        {
            var KeyControl = ctx.control as KeyControl;
            SwitchAttackKey(KeyControl);

        };
    }

    private void SwitchAttackKey(KeyControl KeyControl)
    {
        switch (KeyControl.keyCode)
        {
            case Key.Digit1:
                CycleAttack(1);
                break;
            case Key.Digit2:
                CycleAttack(2);
                break;
            case Key.Digit3:
                CycleAttack(3);
                break;
            case Key.Digit4:
                CycleAttack(4);
                break;
            case Key.Digit5:
                CycleAttack(5);
                break;
            default:
                break;
        }
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void CycleAttackScroll(int direction) 
    {
        currentAttackIndex += direction;
        currentAttackIndex = Mathf.Clamp(currentAttackIndex, 0, mSkillUIList.Count - 1);
        SelectAbility(currentAttackIndex);
    }
    private void CycleAttack(int index)
    {
        if (index >= mSkillUIList.Count +1 ) { return; }
        currentAttackIndex = (index - 1);
        SelectAbility(currentAttackIndex);
        //Debug.Log($"Switched to attack: {currentAttackIndex}");
    }
}
