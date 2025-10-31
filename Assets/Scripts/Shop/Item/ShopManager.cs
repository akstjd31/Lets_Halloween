using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

enum PlayStyle
{
    Human,
    Monster
}

public class ShopManager : MonoBehaviour
{
    [Header("유닛 목록")]
    public PlayerUnit[] units;
    [Header("스킬 목록")]
    public SkillBase[] skills;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI[] infoText;

    public string[] textData;

    private int itemIndex;

    public int gold = 1000;

    [SerializeField] Transform list;
    [SerializeField] GameObject itemPrefab;

    private bool checktype;

    private SkillButton _setButton;

    PlayStyle style;

    public void SetupIndex(int index)
    {
        itemIndex = index;
    }

    public void UnitInfo()
    {
        infoText[0].text = units[itemIndex].name; // 이름
        infoText[1].text = units[itemIndex].name; // 공격력
        infoText[2].text = units[itemIndex].name; // 공격속도
        infoText[3].text = units[itemIndex].name; // 공격타입
        infoText[4].text = units[itemIndex].name; // 이름
        infoText[5].text = units[itemIndex].name; // 이름
    }
    public void SkillInfo()
    {
        Debug.Log("처음 잘 불러와짐");
        infoText[0].text = skills[itemIndex].skillName; // 이름
        Debug.Log("스킬 이름 불러와짐");
        infoText[1].text = skills[itemIndex].name; // 효과
        Debug.Log("스킬 효과 불러와짐");
        infoText[2].text = skills[itemIndex].name; // 스킬
        Debug.Log("스킬 스킬 불러와짐");
        infoText[3].text = skills[itemIndex].skillCooldown.ToString(); // 쿨타임
        Debug.Log("스킬 쿨타임 불러와짐");
        infoText[4].text = skills[itemIndex].skillDuration.ToString(); // 지속시간
        Debug.Log("스킬 지속시간 불러와짐");
        infoText[5].text = skills[itemIndex].skillprice.ToString(); // 가격
        Debug.Log("스킬 가격 불러와짐");
    }

    public void Buy()
    {
       
        int price = checktype? skills[itemIndex].skillprice : 500;

        buttonText.text = "구입 가능";
     
        if(price > gold)
        {
            buttonText.text = "금액 부족";
            return;
        }
        
        gold -= price;

        if (checktype == true)
        {
            _setButton.AddCount();
        }

        else if (checktype == false)        
        {
            MouseTrackingManager.Instance.SpawnTargetUnit(units[itemIndex]);
        }

    }

    public void SkillCheck()
    {
        checktype = true;
    }

    public void UnitCheck()
    {
        checktype = false;
    }

    public void SetSkillButton(SkillButton setButton)
    {
        _setButton = setButton;
    }
}
