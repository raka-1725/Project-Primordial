using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


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

    [SerializeField] private float ScaleUpWidth = 90;
    [SerializeField] private float ScaleUpHeight = 90;

    [SerializeField] private float DefaultWidth = 50;
    [SerializeField] private float DefaultHeight = 50;

    [SerializeField] private List<GameObject> mSkillUIList;
    [SerializeField] private List<Sprite> mSkillIconSprites; //change this once there is sprite variable in the scriptable object
    private int currentSelectedIndex;
    private InputSystem_Actions inputActions;

    public void NewSkillAccuired(SMagicAttackData magicdata) 
    {
        GameObject newSkillIconObj = Instantiate(mSkillIconPrefab, mSkillSliderParent);
        newSkillIconObj.GetComponent<SkillIcon>().UpdateIcon(mSkillIconSprites[0], magicdata.mAttackName); 
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
            switch (KeyControl.keyCode) 
            {
                case Key.Digit1: CycleAttack(1); break;
                case Key.Digit2: CycleAttack(2); break;
                case Key.Digit3: CycleAttack(3); break;
                case Key.Digit4: CycleAttack(4); break;
                case Key.Digit5: CycleAttack(5); break;
            }
        };
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void CycleAttackScroll(int direction) 
    {
        currentAttackIndex += direction;
        currentAttackIndex = Mathf.Clamp(currentAttackIndex, 0, mSkillUIList.Count - 1);
        SelectAbility(currentAttackIndex);
    }
    private void CycleAttack(int direction)
    {
        currentAttackIndex = (direction - 1);
        SelectAbility(currentAttackIndex);
        Debug.Log($"Switched to attack: {currentAttackIndex}");
    }
}
