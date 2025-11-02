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
    // [Header("���� ���")]
    public PlayerUnit[] units;
    // [Header("��ų ���")]
    public SkillBase[] skills;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI[] infoText;
    public PlayerUnit playerUnit;

    public string[] textData;

    private int itemIndex;

    public int gold = 1000;

    private bool checktype;

    private SkillButton _setButton;

    PlayStyle style;

    private GameObject _preview;

    private void Start()
    {
        //gold = (GameManager.Instance.gameOptionData.unit as Player).Money;
    }

    public void SetupIndex(int index)
    {
        itemIndex = index;
    }

    public void UnitInfo()
    {
        infoText[0].text = units[itemIndex].name; // �̸�
        infoText[1].text = units[itemIndex].name; // ���ݷ�
        infoText[2].text = units[itemIndex].name; // ���ݼӵ�
        infoText[3].text = units[itemIndex].name; // ����Ÿ��
        infoText[4].text = units[itemIndex].name; // �̸�
        infoText[5].text = units[itemIndex].name; // �̸�
    }
    public void SkillInfo()
    {
        Debug.Log("ó�� �� �ҷ�����");
        infoText[0].text = skills[itemIndex].skillName; // �̸�
        Debug.Log("��ų �̸� �ҷ�����");
        infoText[1].text = skills[itemIndex].name; // ȿ��
        Debug.Log("��ų ȿ�� �ҷ�����");
        infoText[2].text = skills[itemIndex].name; // ��ų
        Debug.Log("��ų ��ų �ҷ�����");
        infoText[3].text = skills[itemIndex].skillCooldown.ToString(); // ��Ÿ��
        Debug.Log("��ų ��Ÿ�� �ҷ�����");
        infoText[4].text = skills[itemIndex].skillDuration.ToString(); // ���ӽð�
        Debug.Log("��ų ���ӽð� �ҷ�����");
        infoText[5].text = skills[itemIndex].skillPrice.ToString(); // ����
        Debug.Log("��ų ���� �ҷ�����");
    }

    public void Buy()
    {
        int price = checktype? skills[itemIndex].skillPrice : 500;

        //buttonText.text = "���� ����";
     
        // if(price > gold)
        // {
        //     buttonText.text = "�ݾ� ����";
        //     return;
        // }
        
        gold -= price;

        if (checktype == true)
        {
            _setButton.AddCount();
        }

        else if (checktype == false)        
        {
            MouseTrackingManager.Instance.targetObject = _preview;
            MouseTrackingManager.Instance.SpawnTargetUnit(units[itemIndex]);
            Debug.Log("프리뷰" + _preview);
        }
        Debug.Log("현재 체크" + checktype);
        Debug.Log("현재" + units[itemIndex]);

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
    public void TargetObjDestroy()
    {
        MouseTrackingManager.Instance.targetDestory();
    }

    public void SetPreview(GameObject preview)
    {
        _preview = preview;
    }
}
