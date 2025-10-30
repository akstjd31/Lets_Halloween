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
    }

    public void OnEasyButtonClick()
    {
        if (!isFirstClicked)
        {
            isFirstClicked = true;
            ChooseButton.SetActive(true);
        }
    }
    public void OnNormalClick()
    {
        if (!isFirstClicked)
        {
            isFirstClicked = true;
            ChooseButton.SetActive(true);
        }
    }
    public void OnHardButtonClick()
    {
        if (!isFirstClicked)
        {
            isFirstClicked = true;
            ChooseButton.SetActive(true);
        }
    }
}
