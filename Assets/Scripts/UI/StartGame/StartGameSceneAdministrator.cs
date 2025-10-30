using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameSceneAdministrator : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button inputStartButton;
    [SerializeField] private Button inputRuelButton;
    [SerializeField] private Button inputQuiteButton;
    [Header("패널")]
    [SerializeField] private GameObject inputRuelPanels;
    [Header("카메라")]
    [SerializeField] private GameObject inputMainCamera;
    [SerializeField] private GameObject inputSubCamera;
    [Header("캔버스")]
    [SerializeField] private GameObject inputMainCanvas;
    [SerializeField] private GameObject inputSubCanvas;
    [SerializeField] private GameObject inputSpawnManager;

    static public bool IsFirstPlayingStoryPlot = true;

    private int changeCameraCount = 0;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        

        // 스폰 매니저가 진행 중인 도중 아래 내용 진행하지 않음
        if (inputSpawnManager.activeSelf) { return; }

        // 데이터 매니저에 줄거리 이행 내역 체크
        IsFirstPlayingStoryPlot = false;
        //Debug.Log($"테스트 : {DataManager.IsFirstPlayingStoryPlot}");

        ChangeCamera();
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
        Debug.Log($"{IsFirstPlayingStoryPlot}");
        // 처음 줄거리 보는 것이 아닐 경우 카메라 전환 및 패널 비활성화
        if (!IsFirstPlayingStoryPlot) { ChangeCamera(); inputRuelPanels.SetActive(false); }
        else
        {
            // 게임 오브젝트를 명시적으로 활성화 함
            gameObject.SetActive(true);
            // 서브 카메라 활성화 
            inputSubCamera.SetActive(true);
            // 메인 카메라 비활성화
            inputMainCamera.SetActive(false);
            //서브 캔버스 활성화
            inputSubCanvas.SetActive(true);
            // 메인 캔버스 비활성화
            inputMainCanvas.SetActive(false);
            // 패널 UI 비활성화
            inputRuelPanels.SetActive(false);
        }

    }

    public void OnStartButtonClick()
    {
        Debug.Log("시작 버튼이 눌렸습니다.");
        // 셋 업 게임 씬 로드
        SceneManager.LoadScene(1);
        
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

    private void ChangeCamera()
    {
        if(changeCameraCount < 1)
        {
            // 카메라 전환
            inputMainCamera.SetActive(true);
            inputSubCamera.SetActive(false);
            // 캔버스 전환
            inputMainCanvas.SetActive(true);
            inputSubCanvas.SetActive(false);
            // 데이터 매니저에 줄거리 이행 내역 체크
            if (IsFirstPlayingStoryPlot) { IsFirstPlayingStoryPlot = false; }
        }
        // 처음이 아닌 경우 넘어간다.
        else { return; }
    }

}
