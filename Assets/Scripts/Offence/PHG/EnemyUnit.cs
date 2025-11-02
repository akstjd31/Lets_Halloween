using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    //프로토타입
    [SerializeField] private float moveSpeed;   //회전속도는 이동속도와 같게 맞출것
    private int currentWayPointIndex = 0;

    private Transform wayPointBox;
    [SerializeField] private int maxHp;
    [SerializeField] private string EnemyName;

    private int currentHp;

    Animator animator;

    bool isDie = false;     //유닛 생사여부확인


    private void Start()
    {
        wayPointBox = GameObject.Find("WayPoint").transform;
        animator = GetComponent<Animator>();
        currentHp = maxHp;
    }


    //웨이포인트 충돌처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("WayPoint"))
        {
            if (wayPointBox == null) { return; }

            currentWayPointIndex++;

            if (currentWayPointIndex >= wayPointBox.childCount) { currentWayPointIndex = 0; }
        }

        if (other.tag == "EndPoint")
        {
            Debug.Log($"{gameObject.name} 끝지점 도달");
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        MoveObj();
    }

    //오브젝트 이동
    private void MoveObj()
    {
        if (wayPointBox == null) { return; }

        Transform targetTransform = wayPointBox.GetChild(currentWayPointIndex);
        Vector3 direction = (targetTransform.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position,targetTransform.position,moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), moveSpeed * Time.deltaTime);

        if (direction == Vector3.zero)
        {
            currentWayPointIndex++;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"데미지 {damage}입음");
        currentHp = currentHp - damage;

        if (currentHp <= 0 && isDie==false)
        {
            moveSpeed = 0;
            isDie = true;
            animator.SetTrigger("Die");
        }
    }
 
    //생성시 플레이어가 가는위치 던져줌
    public void SetWayPoint(int wayPointIndex)
    {
        currentWayPointIndex = wayPointIndex;
    }

    //유니티 이벤트 함수
    void Die()
    {
       gameObject.SetActive(false);   
    }

  

}
