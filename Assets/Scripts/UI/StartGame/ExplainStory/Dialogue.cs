using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    //[SerializeField] private Transform startPosition;

    private Vector3 moveForward = Vector3.zero;

    private void Start()
    {
        Init();
    }


    private void Update()
    {
        Move();
    }

    // 충돌 처리 충돌 상태에서 벗어날 때만 실행
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollidingBody"))
        {
            Debug.Log("다이얼로그가 충돌체와의 벗어났습니다.");
            // 충돌에서 벗어나면 게임 오브젝트 비활성화
            gameObject.SetActive(false);
        }
    }


    // 다이얼로그 이동 구현
    private void Move()
    {
        transform.position += moveForward * moveSpeed * Time.deltaTime;
    }

    
    private void Init()
    {
        // 테스트 할 때만 활성화 추후 다른 곳에서 활성화할 예정
        //gameObject.SetActive(true);
        // 이동방향 입력
        moveForward += Vector3.forward;
        moveForward += Vector3.up;
        // 테스트 할 때만 사용하는 매서드
        //SetStartPosition();
    }

    //private void  SetStartPosition()
    //{
    //    transform.position = startPosition.position;
    //}

    
}
