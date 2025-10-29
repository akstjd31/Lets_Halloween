using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameSceneAdministrator : MonoBehaviour
{
    [SerializeField] private Button inputStartButton;
    [SerializeField] private Button inputOptionButton;
    [SerializeField] private Button inputQuiteButton;



    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        inputStartButton.onClick.AddListener(OnStartButtonClick);
        inputQuiteButton.onClick.AddListener(OnQuiteBottonClick);
        inputOptionButton.onClick.AddListener(OnOptionButtonClick);
        
    }

    private void OnDestroy()
    {
        inputStartButton.onClick.RemoveListener(OnStartButtonClick);
        inputQuiteButton.onClick.RemoveListener(OnQuiteBottonClick);
        inputQuiteButton.onClick.RemoveListener(OnQuiteBottonClick);
    }

    // 게임 오브젝트를 명시적으로 활성화 함
    private void Init() { gameObject.SetActive(true); }

    public void OnStartButtonClick()
    {
        Debug.Log("시작 버튼이 눌렸습니다.");
    }

    public void OnOptionButtonClick()
    {
        Debug.Log("옵션 버튼이 눌렸습니다.");
    }

    public void OnQuiteBottonClick()
    {
        Debug.Log("나가기 버튼이 눌렸습니다.");
    }

    
}
