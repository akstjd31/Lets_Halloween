using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public enum ShopCheck
{
    Unit = 1,
    Monster,
    Skill
}

public class ShopManager : MonoBehaviour
{
    [Header("에너미 슬롯")]
    public EnemyUnit[] enemyUnits;
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
    public string rawImageName;
    public string renderCameraName;

    [SerializeField] private Camera renderCamera;

    [SerializeField] private RawImage rawImage;

    Unit unit;

    // [SerializeField] private int _money = 1000;

    private int itemIndex;


    int price;

    private SkillButton _setButton;
    private MonsterUnitButton _setUnitButton;

    [SerializeField] public ShopCheck checker;

    private GameObject _preview;

    private string lastButtonText = "";
    UIManager uiManager;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void Start()
    {
        unit = GameManager.Instance.gameOptionData.unit;
        SetRender();
    }

    public void SetupIndex(int index)
    {
        itemIndex = index;
        UpdateBuyText();
    }

    public void EnemyInfo()
    {
        infoText_Name.text = enemyUnits[itemIndex].Name; // 이름
        infoText_SkillNameOrUnitATK.text = enemyUnits[itemIndex].Story; // 배경설명
        infoText_SkillEffectOrUnitATKSPD.text = enemyUnits[itemIndex].Any; // 공격속도
        infoText_SkillCoolTimeOrUnitPassiveName.text = "체력 : " + enemyUnits[itemIndex].HP.ToString(); // 체력
        infoText_SkillDurationOrUnitPassiveEffect.text = "이동 속도 : " + enemyUnits[itemIndex].MoveSpeed.ToString(); // 이동 속도
        infoText_Price.text = "가격 : " + enemyUnits[itemIndex].Price.ToString(); // 가격
    }
    public void UnitInfo()
    {
        infoText_Name.text = units[itemIndex].Name; // 이름
        infoText_SkillNameOrUnitATK.text = "공격력  : " + units[itemIndex].Power.ToString(); // 공격력
        infoText_SkillEffectOrUnitATKSPD.text = "공격 속도 : " + units[itemIndex].AttackDelay.ToString() + "초"; ; // 공격속도
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
        switch (checker)
        {
            case ShopCheck.Skill:
                price = skills[itemIndex].skillPrice;
                break;

            case ShopCheck.Unit:
                price = units[itemIndex].Price;
                break;

            case ShopCheck.Monster:
                price = enemyUnits[itemIndex].Price;
                break;
        }

        // string newText = (price > player.Money) ? "돈 부족" : "구입 가능";

        //if (newText != lastButtonText)
        //{
        //    buttonText.text = newText;
        //    lastButtonText = newText;
        //}
    }

    public void Buy()
    {
        Debug.Log("현재 열거형"+checker.ToString());
        switch (checker)
        {
            case ShopCheck.Skill:

                price = skills[itemIndex].skillPrice;

                if (price > unit.Money)
                {
                    buttonText.text = "돈 부족";
                    return;
                }

                _setButton.AddCount();
                unit.ReceiveReward(-price);
                uiManager.UpdateMoney(unit);

                break;

            case ShopCheck.Unit:
                price = units[itemIndex].Price;

                if (price > unit.Money)
                {
                    buttonText.text = "돈 부족";
                    return;
                }

                MouseTrackingManager.Instance.targetObject = _preview;
                MouseTrackingManager.Instance.OnUnitPlaced = OnUnitPlaced;
                MouseTrackingManager.Instance.SpawnTargetUnit(units[itemIndex]);
                break;

            case ShopCheck.Monster:
                price = enemyUnits[itemIndex].Price;

                if (price > unit.Money)
                {
                    buttonText.text = "돈 부족";
                    return;
                }

                _setUnitButton.AddCount();
                unit.ReceiveReward(-price);
                uiManager.UpdateMoney(unit);
                break;
        }
        UpdateBuyText();
    }
    // 관련된 스킬 버튼 설정

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
        unit.ReceiveReward(-price);
        uiManager.UpdateMoney(unit);
        UpdateBuyText();
    }

    public void SetButton(string buttonName)
    {
        switch (checker)
        {
            case ShopCheck.Skill:
                _setButton = GameObject.Find(buttonName).GetComponent<SkillButton>();
                break;

            case ShopCheck.Unit:
                
                break;

            case ShopCheck.Monster:
                _setUnitButton = GameObject.Find(buttonName).GetComponent<MonsterUnitButton>();
                break;
        }
    }

    public void SetRender()
    {
        rawImage = GameObject.Find(rawImageName).GetComponent<RawImage>();
        renderCamera = GameObject.Find(renderCameraName).GetComponent<Camera>();
    }

    public void SetEnum(ShopCheck type)
    {
        checker = type;
    }

    public void SetEnum_Unit()
    {
        SetEnum(ShopCheck.Unit);
    }
    public void SetEnum_Monster()
    {
        SetEnum(ShopCheck.Monster);
    }
    public void SetEnum_Skill()
    {
        SetEnum(ShopCheck.Skill);
    }

}
