using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [Header("카메라")]
    [SerializeField] private GameObject inputCamera;
    [Header("캐릭터")]
    [SerializeField] private GameObject inputPlayer;
    [SerializeField] private GameObject inputEnemy;
    [Header("회전 속도")]
    [SerializeField] private float spinSpeed;
    [Header("애니메이션 컨트롤")]
    [SerializeField] private Animator inputPlayerAnimator;
    [SerializeField] private Animator inputEnemyAnimator;
    [Header("애니매이션 딜레이")]
    [SerializeField] private float animationDelay;
    [Header("셋 업 게임 씬 매니저")]
    [SerializeField] private SetUpGameSceneManager inputGameSceneManager;
    private float delayTiemr;
    private float delayTimerForUpdate;
    private int progressNumber;
    private SetUpGameSceneManager tempManager;

    private bool isUsed = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        //Spin();
        delayTimerForUpdate += Time.deltaTime;
        // 애니메이션 상태 갱신
        //if (delayTimerForUpdate > animationDelay) { UpdateAnimation();}
        

        // 매서드를 사용했다면 타이머 갱신
        if (isUsed) {  delayTiemr += Time.deltaTime; }
        // 딜레이가 끝나면 타미어와 애니매이터 변수 초기화 및 카메라 전환 함수 호출
        if (delayTiemr > animationDelay) 
        { 
            isUsed = false;
            delayTiemr = 0;
            inputEnemyAnimator.SetBool("isClicked", false);
            inputPlayerAnimator.SetBool("isClicked", true);
            inputGameSceneManager.ChangeCamera();
        }
    }

    private void Spin()
    {
        //Debug.Log("회전 실행 중");
        inputPlayer.transform.Rotate(Vector3.up, spinSpeed *  Time.deltaTime);
        inputEnemy.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        
    }

    public void EnemyAttack()
    {
        // 캐릭터가 카메라를 바라보도록 설정
        //inputEnemy.transform.LookAt(inputCamera.transform);
        //inputEnemyAnimator.SetBool("Loop", false);
        inputEnemyAnimator.SetBool("isClicked", true);
        isUsed = true;
    }

    public void PlayerAttack()
    {
        inputPlayerAnimator.SetBool("isClicked", true);
        isUsed = true;
    }

    private void Init()
    {
        // 셋 업 게임 씬 매니저 참조
        tempManager = GetComponent<SetUpGameSceneManager>();
    }

    private void UpdateAnimation()
    {
        // 진행된 업데이트 상황에 맞는 에니메이터 변수 설정
        switch (progressNumber)
        {
            case 0:
                inputEnemyAnimator.SetFloat("progressNumber", 1.1f);
                progressNumber++;
                break;
            case 1:
                inputEnemyAnimator.SetFloat("progressNumber", 2.1f);
                progressNumber++;
                break;
            case 2:
                inputEnemyAnimator.SetFloat("progressNumber", 3.1f);
                progressNumber++;
                break;
            case 3:
                inputEnemyAnimator.SetFloat("progressNumber", 3.1f);
                progressNumber--;
                break;

        }
    }
}
