using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("시작 지점")]
    [SerializeField] private Transform startPosition;

    [Header("입력 받을 대사들")]
    [SerializeField] private GameObject Dialogue1;
    [SerializeField] private GameObject Dialogue2;
    [SerializeField] private GameObject Dialogue3;
    [SerializeField] private GameObject Dialogue4;
    [SerializeField] private GameObject Dialogue5;

    [Header("속성")]
    [SerializeField] private float spawnDelay = 1f;
    private float spawnTimer = 0f;
    private int numberOfLines = 0;
    private GameObject tempDialogue;
    private bool isIdleState = false;

    // 입력 받은 다이얼로그를 저장할 큐 선언
    private Queue<GameObject> dialogues = new Queue<GameObject>();

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnDelay)
        {
            RestartSpawnDialogue();
            SpawnDialogue();
            // 컴퓨터가 현재 진행 중인 맥락을 알 수 있도록 현재 줄 위치를 센다.
            numberOfLines++;
            StopSpawnDialogue();
        }
    }

    private void Init()
    {
        // 다이얼로그를 비활성화 시킨다.
        Dialogue1.SetActive(false);
        Dialogue2.SetActive(false);
        Dialogue3.SetActive(false);
        Dialogue4.SetActive(false);
        Dialogue5.SetActive(false);

        // 큐에 다이얼로그를 넣어준다.
        dialogues.Enqueue(Dialogue1);
        dialogues.Enqueue(Dialogue2);
        dialogues.Enqueue(Dialogue3);
        dialogues.Enqueue(Dialogue4);
        dialogues.Enqueue(Dialogue5);
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

}
