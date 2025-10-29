using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameSceneAdministrator : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button inputStartButton;
    [SerializeField] private Button inputRuelButton;
    [SerializeField] private Button inputQuiteButton;
    [Header("패널")]
    [SerializeField] private GameObject inputRuelPanels;


    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        inputStartButton.onClick.AddListener(OnStartButtonClick);
        inputRuelButton.onClick.AddListener(OnRuelButtonClick);
        inputQuiteButton.onClick.AddListener(OnQuiteBottonClick);
        
    }

    private void OnDestroy()
    {
        inputStartButton.onClick.RemoveListener(OnStartButtonClick);
        inputQuiteButton.onClick.RemoveListener(OnRuelButtonClick);
        inputQuiteButton.onClick.RemoveListener(OnQuiteBottonClick);
    }

    
    private void Init() 
    {
        // 게임 오브젝트를 명시적으로 활성화 함
        gameObject.SetActive(true); 
        // 패널 UI 비활성화
        inputRuelPanels.SetActive(false);
    }

    public void OnStartButtonClick()
    {
        Debug.Log("시작 버튼이 눌렸습니다.");
    }

    public void OnRuelButtonClick()
    {
        Debug.Log("플레이 방식 버튼이 눌렸습니다.");
        inputRuelPanels.SetActive(true);
    }

    public void OnQuiteBottonClick()
    {
        Debug.Log("나가기 버튼이 눌렸습니다.");
        QuitGame();
    }

    // 실행시 프로그램 종료
    public void QuitGame()
    {
       #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
