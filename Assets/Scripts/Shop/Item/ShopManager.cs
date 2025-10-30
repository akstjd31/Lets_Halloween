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

    public int[] itemPrice;

    public int gold = 1000;

    [SerializeField] Transform list;
    [SerializeField] GameObject itemPrefab;

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
        infoText[0].text = skills[itemIndex].name; // 이름
        infoText[1].text = skills[itemIndex].name; // 효과
        infoText[2].text = skills[itemIndex].name; // 스킬
        infoText[3].text = skills[itemIndex].name; // 쿨타임
        infoText[4].text = skills[itemIndex].name; // 지속시간
        infoText[5].text = skills[itemIndex].name; // 가격
    }

    public void Buy()
    {
        int price = itemPrice[itemIndex];

        buttonText.text = "구입 가능";
     
        if(price > gold)
        {
            buttonText.text = "금액 부족";
            return;
        }
        
        gold -= price;


    }
}
