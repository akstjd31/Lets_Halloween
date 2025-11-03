using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


enum PlayStyle
{
    Human,
    Monster
}

public class ShopManager : MonoBehaviour
{
    [Header("유닛 슬롯")]
    public PlayerUnit[] units;
    [Header("스킬 슬롯")]
    public SkillBase[] skills;
    [Header("구매 버튼")]
    public TextMeshProUGUI buttonText;
    [Header("정보 텍스트_이름")]
    public TextMeshProUGUI infoText_Name;
    [Header("정보 텍스트_스킬 명 or 유닛 배경")]
    public TextMeshProUGUI infoText_SkillNameOrUnitATK;
    [Header("정보 텍스트_스킬 효과 or 유닛 공격력")]
    public TextMeshProUGUI infoText_SkillEffectOrUnitATKSPD;
    [Header("정보 텍스트_스킬 쿨타임 or 유닛 공격속도")]
    public TextMeshProUGUI infoText_SkillCoolTimeOrUnitPassiveName;
    [Header("정보 텍스트_스킬 지속시간 or 유닛 공격범위")]
    public TextMeshProUGUI infoText_SkillDurationOrUnitPassiveEffect;
    [Header("정보 텍스트_가격")]
    public TextMeshProUGUI infoText_Price;
    [Header("아이템이미지")]
    public RawImage rawImage;

    [SerializeField] private Camera renderCamera;

    Player player;

    private int itemIndex;

    private bool checktype;

    int price;

    private SkillButton _setButton;

    PlayStyle style;

    private GameObject _preview;

    private string lastButtonText = "";

    private void Start()
    {
        player = (GameManager.Instance.gameOptionData.unit as Player);
    }

    public void SetupIndex(int index)
    {
        itemIndex = index;
        UpdateBuyText();
    }

    public void UnitInfo()
    {
        infoText_Name.text = units[itemIndex].Name; // 이름
        infoText_SkillNameOrUnitATK.text = "공격력  : " + units[itemIndex].Power.ToString(); // 공격력
        infoText_SkillEffectOrUnitATKSPD.text = "공격 속도 : " + units[itemIndex].AttackDelay.ToString()+"초"; ; // 공격속도
        infoText_SkillCoolTimeOrUnitPassiveName.text = units[itemIndex].PassiveName; // 패시브명
        infoText_SkillDurationOrUnitPassiveEffect.text = units[itemIndex].PassiveEffect; // 패시브효과
        infoText_Price.text = "가격 : " + units[itemIndex].Price.ToString(); // 가격
    }
    public void SkillInfo()
    {

        infoText_Name.text = skills[itemIndex].skillName; // 이름

        infoText_SkillNameOrUnitATK.text = skills[itemIndex].skillEffectName; // 스킬효과명

        infoText_SkillEffectOrUnitATKSPD.text = skills[itemIndex].skillEffect; // 스킬효과

        infoText_SkillCoolTimeOrUnitPassiveName.text = "쿨타임 : " + skills[itemIndex].skillCooldown.ToString(); // 쿨타임

        infoText_SkillDurationOrUnitPassiveEffect.text = "지속시간 : " + skills[itemIndex].skillDuration.ToString(); // 지속시간

        infoText_Price.text = "가격 : " + skills[itemIndex].skillPrice.ToString(); // 가격

    }

    public void UpdateBuyText()
    {
        if (checktype)
        {
            price = skills[itemIndex].skillPrice;
        }
        else
        {
            price = units[itemIndex].Price;
        }

        string newText = (price > player.Money) ? "돈 부족" : "구입 가능";

        if (newText != lastButtonText)
        {
            buttonText.text = newText;
            lastButtonText = newText;
        }
    }

    public void Buy()
    {
        price = checktype? skills[itemIndex].skillPrice : units[itemIndex].Price;

        Debug.Log("남은돈"+player.Money);

        if(price > player.Money)
        {
             buttonText.text = "돈 부족";
             return;
        }

        if (checktype == true)
        {
            player.ReceiveReward(-price);
            _setButton.AddCount();

        }

        else if (checktype == false)        
        {
            MouseTrackingManager.Instance.targetObject = _preview;
            MouseTrackingManager.Instance.OnUnitPlaced = OnUnitPlaced;
            MouseTrackingManager.Instance.SpawnTargetUnit(units[itemIndex]);
        }

        UpdateBuyText();
    }
    // 스킬인지 체크
    public void SkillCheck()
    {
        checktype = true;
    }
    // 유닛인지 체크
    public void UnitCheck()
    {
        checktype = false;
    }
    // 관련된 스킬 버튼 설정
    public void SetSkillButton(SkillButton setButton)
    {
        _setButton = setButton;
    }
    // 유닛 프리뷰 설정
    public void SetPreview(GameObject preview)
    {
        _preview = preview;
    }
    // 캐릭터 모습 보여줄 카메라 위치 설정
    public void SetCameraPosition(int positionX)
    {
        renderCamera.transform.position = new Vector3(positionX, renderCamera.transform.position.y, renderCamera.transform.position.z);
    }

    private void OnUnitPlaced()
    {
        price = units[itemIndex].Price;
        player.ReceiveReward(-price);
        UpdateBuyText();
    }
}
