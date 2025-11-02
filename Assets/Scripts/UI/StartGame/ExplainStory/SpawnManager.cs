using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("카메라")]
    [SerializeField] private GameObject inputMainCamera;
    [SerializeField] private GameObject inputSubCamera;

    [Header("캔버스")]
    [SerializeField] private GameObject inputMainCanvas;
    [SerializeField] private GameObject inputSubCanvas;

    [Header("타이틀")] 
    [SerializeField] private GameObject inputTitle;
    private Transform tempVector3;
    private int titleCount;

    [Header("시작 지점")]
    [SerializeField] private Transform startPosition;

    [Header("입력 받을 대사들")]
    [SerializeField] private GameObject Dialogue1;
    [SerializeField] private GameObject Dialogue2;
    [SerializeField] private GameObject Dialogue3;
    [SerializeField] private GameObject Dialogue4;
    [Header("")]
    [SerializeField] private GameObject Dialogue5;
    [SerializeField] private GameObject Dialogue6;
    [SerializeField] private GameObject Dialogue7;
    [SerializeField] private GameObject Dialogue8;
    [Header("")]
    [SerializeField] private GameObject Dialogue9;
    [SerializeField] private GameObject Dialogue10;
    [SerializeField] private GameObject Dialogue11;
    [SerializeField] private GameObject Dialogue12;
    [SerializeField] private GameObject Dialogue13;
    [Header("")]
    [SerializeField] private GameObject Dialogue14;
    [SerializeField] private GameObject Dialogue15;
    [SerializeField] private GameObject Dialogue16;
    [SerializeField] private GameObject Dialogue17;
    [SerializeField] private GameObject Dialogue18;
    [SerializeField] private GameObject Dialogue19;
    [Header("적 진영 설명 대사")]
    [SerializeField] private GameObject Dialogue20;
    [SerializeField] private GameObject Dialogue21;
    [SerializeField] private GameObject Dialogue22;
    [SerializeField] private GameObject Dialogue23;
    [SerializeField] private GameObject Dialogue24;
    [SerializeField] private GameObject Dialogue25;

    [Header("페이드")]
    [SerializeField] private GameObject tempFadeIn;

    [Header("속성")]
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float cameraChangeDelay = 5f;
    private float spawnTimer = 0f;
    private int numberOfLines = 0;
    private GameObject tempDialogue;
    private bool isIdleState = false;
    private float cameraDelayTiemr = 0f;


    // 입력 받은 다이얼로그를 저장할 큐 선언
    private Queue<GameObject> dialogues = new Queue<GameObject>();

    private void Awake()
    {
        Init();
    }


    private void Update()
    {
        // 스페이스바 키를 입력 받으면 캔버스, 카메라 전환
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputSubCamera.SetActive(false);
            inputSubCanvas.SetActive(false);
            inputMainCamera.SetActive(true);
            inputMainCanvas.SetActive(true);
            StartGameSceneAdministrator.IsFirstPlayingStoryPlot = false;
        }

        // 페이드 인 종료되면 타이틀 활성화
        if ( !tempFadeIn.activeSelf && titleCount < 1) 
        { 
            StartTitle(); 
            titleCount++;
        }
        // 타이틀이 끝나지 않은 경우 아래 실행하지 않음
        if (inputTitle.activeSelf) { return; }

        spawnTimer += Time.deltaTime;

        if(spawnTimer > spawnDelay )
        {
            RestartSpawnDialogue();
            SpawnDialogue();
            
            StopSpawnDialogue();
        }

        // 대기 시간 갱신
        if(dialogues.Count == 0 && gameObject.activeSelf) { cameraDelayTiemr += Time.deltaTime; }
        

        // 대기 시간을 초과하면 생산 매니저 비활성화 및 줄거리 감상 체크
        if( cameraDelayTiemr > cameraChangeDelay)
        {
            StartGameSceneAdministrator.IsFirstPlayingStoryPlot = false;
            gameObject.SetActive(false);
        }

    }

    private void Init()
    {

        #region 타이틀 및 다이얼로그 설정

        // 타이틀 비활성화
        inputTitle.SetActive( false );

        // 다이얼로그를 비활성화 시킨다.
        Dialogue1.SetActive(false);
        Dialogue2.SetActive(false);
        Dialogue3.SetActive(false);
        Dialogue4.SetActive(false);
        Dialogue5.SetActive(false);
        Dialogue6.SetActive(false);
        Dialogue7.SetActive(false);
        Dialogue8.SetActive(false);
        Dialogue9.SetActive(false);
        Dialogue10.SetActive(false);
        Dialogue11.SetActive(false);
        Dialogue12.SetActive(false);
        Dialogue13.SetActive(false);
        Dialogue14.SetActive(false);
        Dialogue15.SetActive(false);
        Dialogue16.SetActive(false);
        Dialogue17.SetActive(false);
        Dialogue18.SetActive(false);
        Dialogue19.SetActive(false);
        Dialogue20.SetActive(false);
        Dialogue21.SetActive(false);
        Dialogue22.SetActive(false);
        Dialogue23.SetActive(false);
        Dialogue24.SetActive(false);
        Dialogue25.SetActive(false);

        // 큐에 다이얼로그를 넣어준다.
        dialogues.Enqueue(Dialogue1);
        dialogues.Enqueue(Dialogue2);
        dialogues.Enqueue(Dialogue3);
        dialogues.Enqueue(Dialogue4);
        dialogues.Enqueue(Dialogue5);
        dialogues.Enqueue(Dialogue6);
        dialogues.Enqueue(Dialogue7);
        dialogues.Enqueue(Dialogue8);
        dialogues.Enqueue(Dialogue9);
        dialogues.Enqueue(Dialogue10);
        dialogues.Enqueue(Dialogue11);
        dialogues.Enqueue(Dialogue12);
        dialogues.Enqueue(Dialogue13);
        dialogues.Enqueue(Dialogue14);
        dialogues.Enqueue(Dialogue15);
        dialogues.Enqueue(Dialogue16);
        dialogues.Enqueue(Dialogue17);
        dialogues.Enqueue(Dialogue18);
        dialogues.Enqueue(Dialogue19);
        dialogues.Enqueue(Dialogue20);
        dialogues.Enqueue(Dialogue21);
        dialogues.Enqueue(Dialogue22);
        dialogues.Enqueue(Dialogue23);
        dialogues.Enqueue(Dialogue24);
        dialogues.Enqueue(Dialogue25);
        #endregion
        // 입력 받은 페이드 컨트롤러 활성화 하기
        tempFadeIn.SetActive(true);
        // 페이드 인관련 콜백함수 연결하기
        //tempFadeIn = GetComponent<FadeIn>();
        //tempFadeIn.OnCompleteCallBack.AddListener(StartTitle);
    }

    private void SpawnDialogue()
    {
        // 만약 큐가 비어있거나 Idle 상태이면 실행 종료
        if(dialogues.Count == 0 || isIdleState )
        {
            return;
        }
        // 셍성한 다이얼 로그는 제외한다.
        tempDialogue = dialogues.Dequeue();
        // 위치를 시작 위치로 설정
        tempDialogue.transform.position = startPosition.position;
        // 다이얼로그 활성화
        tempDialogue.SetActive(true);
        // 컴퓨터가 현재 진행 중인 맥락을 알 수 있도록 현재 줄 위치를 센다.
        numberOfLines++;
        Debug.Log($"{numberOfLines}");
        
    }

    private void StopSpawnDialogue()
    {
        switch (numberOfLines)
        {
            // 문단이 생기면 Idle 상태 값 변환하는 케이스 삽입
            case 4:
                //Idle 상태를 참으로 변환
                isIdleState = true;
                break;
            case 8:
                isIdleState = true;
                break;
            case 13:
                isIdleState = true;
                break;
            case 19:
                isIdleState = true;
                break;
            default:
                // 문단을 띄워야 할 경우가 아니면 넘어간다
                break;

        }
    }

    private void RestartSpawnDialogue()
    {
        // Idle 상태인 경우 빈 줄을 3번 출력 후 상태 및 타이머 초기화
        if ( isIdleState && spawnTimer > 2 * spawnDelay )
        {
            spawnTimer = 0;
            isIdleState = false;
        }
        // Idle 상태가  아닌 경우 원래 스폰 딜레이가 지나면 타이머만 초기화
        else if ( !isIdleState && spawnTimer > spawnDelay) { spawnTimer = 0; }
        // 해당 사항 없을 경우 넘어간다.
        else { return; }
    }

    public void StartTitle()
    {
        Debug.Log("타이틀이 활성화됩니다.");
        // 포지션 정한 후 활성화
        inputTitle.transform.position = inputSubCamera.transform.position ;
        inputTitle.SetActive(true);
    }

    
}
