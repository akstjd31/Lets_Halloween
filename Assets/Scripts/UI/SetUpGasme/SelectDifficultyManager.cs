using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDifficultyManager : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private GameObject easyButton;
    [SerializeField] private GameObject normalButton;
    [SerializeField] private GameObject hardButton;
    [SerializeField] private GameObject ChooseButton;
    [Header("이미지")]
    [SerializeField] private GameObject easyButtonImage;
    [SerializeField] private GameObject normalButtonImage;
    [SerializeField] private GameObject hardButtonImage;
    [Header("캐릭터")]
    [SerializeField] private GameObject easyCharacter;
    [SerializeField] private GameObject normalCharacter;
    [SerializeField] private GameObject hardCharacter;
    [Header("설명문")] 
    [SerializeField] private GameObject easyText;
    [SerializeField] private GameObject normalText;
    [SerializeField] private GameObject hardText;
    [Header("빛")]
    [SerializeField] private GameObject LightForAfternoon;
    [SerializeField] private GameObject LightForMidnight;


    // 버튼이 처음 클릭되었는지 판단할 변수
    private bool isFirstClicked = false;

    // 캔버스 활성화 시 초기에 설정될 것들
    private void OnEnable()
    {
        // 명시적으로 버튼 활성화 설정
        easyButton.SetActive(true);
        normalButton.SetActive(true);
        hardButton.SetActive(true);
        ChooseButton.SetActive(false);
        // 이미지는 비활성화
        easyButtonImage.SetActive(false);
        normalButtonImage.SetActive(false);
        hardButtonImage.SetActive(false);
        // 캐릭터 비활성화
        easyCharacter.SetActive(false);
        normalCharacter.SetActive(false);
        hardCharacter.SetActive(false);
        // 텍스트 비활성화 
        easyText.SetActive(false);
        normalText.SetActive(false);
        hardText.SetActive(false);
        // 초기 빛 설정
        LightForAfternoon.SetActive(true);
        LightForMidnight.SetActive(false);
    }
    private void Awake()
    {
        //Init();
    }

    private void Init()
    {
        // 명시적으로 버튼 활성화 설정
        easyButton.SetActive(true);
        normalButton.SetActive(true);
        hardButton.SetActive(true);
        ChooseButton.SetActive(false);
        // 이미지는 비활성화
        easyButtonImage.SetActive(false);
        normalButtonImage.SetActive(false);
        hardButtonImage.SetActive(false);
        // 캐릭터 비활성화
        easyCharacter.SetActive(false);
        normalCharacter.SetActive(false);
        hardCharacter.SetActive(false);
    }

    public void OnEasyButtonClick()
    {
        // 처음 클릭을 한 경우에만 진행
        if (!isFirstClicked)
        {
            isFirstClicked = true;
            ChooseButton.SetActive(true);
        }
        // 관련 이미지만 비활성화 하고 나머지는 활성화
        easyButtonImage.SetActive(true);
        normalButtonImage.SetActive(false);
        hardButtonImage.SetActive(false);
        // 해당 캐릭터만 활성화
        easyCharacter.SetActive(true);
        normalCharacter.SetActive(false);
        hardCharacter.SetActive(false);
        // 해당 텍스트만 활성화
        easyText.SetActive(true);
        normalText.SetActive(false);
        hardText.SetActive(false);
        // 해당 빛만 활성화
        LightForAfternoon.SetActive(true);
        LightForMidnight.SetActive(false);
    }
    public void OnNormalClick()
    {
        if (!isFirstClicked)
        {
            isFirstClicked = true;
            ChooseButton.SetActive(true);
        }
        easyButtonImage.SetActive(false);
        normalButtonImage.SetActive(true);
        hardButtonImage.SetActive(false);

        easyCharacter.SetActive(false);
        normalCharacter.SetActive(true);
        hardCharacter.SetActive(false);

        easyText.SetActive(false);
        normalText.SetActive(true);
        hardText.SetActive(false);

        LightForAfternoon.SetActive(false);
        LightForMidnight.SetActive(true);
    }
    public void OnHardButtonClick()
    {
        if (!isFirstClicked)
        {
            isFirstClicked = true;
            ChooseButton.SetActive(true);
        }
        easyButtonImage.SetActive(false);
        normalButtonImage.SetActive(false);
        hardButtonImage.SetActive(true);

        easyCharacter.SetActive(false);
        normalCharacter.SetActive(false);
        hardCharacter.SetActive(true);

        easyText.SetActive(false);
        normalText.SetActive(false);
        hardText.SetActive(true);

        LightForAfternoon.SetActive(false);
        LightForMidnight.SetActive(false);
    }
}
