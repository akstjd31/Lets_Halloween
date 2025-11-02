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
    [Header("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½")]
    public PlayerUnit[] units;
    [Header("ï¿½ï¿½Å³ ï¿½ï¿½ï¿½")]
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
        infoText[0].text = units[itemIndex].name; // ÀÌ¸§
        infoText[1].text = units[itemIndex].name; // °ø°Ý·Â
        infoText[2].text = units[itemIndex].name; // °ø°Ý¼Óµµ
        infoText[3].text = units[itemIndex].name; // °ø°ÝÅ¸ÀÔ
        infoText[4].text = units[itemIndex].name; // ÀÌ¸§
        infoText[5].text = units[itemIndex].name; // ÀÌ¸§
    }
    public void SkillInfo()
    {
        Debug.Log("Ã³À½ Àß ºÒ·¯¿ÍÁü");
        infoText[0].text = skills[itemIndex].skillName; // ÀÌ¸§
        Debug.Log("½ºÅ³ ÀÌ¸§ ºÒ·¯¿ÍÁü");
        infoText[1].text = skills[itemIndex].name; // È¿°ú
        Debug.Log("½ºÅ³ È¿°ú ºÒ·¯¿ÍÁü");
        infoText[2].text = skills[itemIndex].name; // ½ºÅ³
        Debug.Log("½ºÅ³ ½ºÅ³ ºÒ·¯¿ÍÁü");
        infoText[3].text = skills[itemIndex].skillCooldown.ToString(); // ÄðÅ¸ÀÓ
        Debug.Log("½ºÅ³ ÄðÅ¸ÀÓ ºÒ·¯¿ÍÁü");
        infoText[4].text = skills[itemIndex].skillDuration.ToString(); // Áö¼Ó½Ã°£
        Debug.Log("½ºÅ³ Áö¼Ó½Ã°£ ºÒ·¯¿ÍÁü");
        infoText[5].text = skills[itemIndex].skillPrice.ToString(); // °¡°Ý
        Debug.Log("½ºÅ³ °¡°Ý ºÒ·¯¿ÍÁü");
    }

    public void Buy()
    {
        int price = checktype? skills[itemIndex].skillPrice : 500;

        //buttonText.text = "ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½";
     
        // if(price > gold)
        // {
        //     buttonText.text = "ï¿½Ý¾ï¿½ ï¿½ï¿½ï¿½ï¿½";
        //     return;
        // }
        
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
