using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyUnit : MonoBehaviour
{
    //프로토타입
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    private int currentWayPointIndex = 0;

    private Transform wayPointBox;
    [SerializeField] private int hp;
    [SerializeField] private string EnemyName;


    //적군진영 플레이어만 이용가능함
    [SerializeField] private GameObject[] myEnemyUnitArray;
    

    private void Start()
    {
        wayPointBox = GameObject.Find("WayPoint").transform;

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

        if(other.tag=="EndPoint")
        {
            Debug.Log($"{gameObject.name} 끝지점 도달");
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        MoveObj();

        if(gameObject.tag==("EnemyUnit"))
        {
            CreateEnemyUnit();
        }

    }

    //오브젝트 이동
    private void MoveObj()
    {
        if (wayPointBox == null) { return; }

        Transform targetTransform = wayPointBox.GetChild(currentWayPointIndex);
        Vector3 direction = (targetTransform.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position,targetTransform.position,moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed *Time.deltaTime);

        if (direction == Vector3.zero)
        {
            currentWayPointIndex++;
        }
      
    }

    //적군진영 플레이어는 적 생성가능
    void CreateEnemyUnit()
    {
        switch(Input.inputString.ToUpper())
        {
            case "Q":
                Instantiate(myEnemyUnitArray[0], transform.position, transform.rotation);
                break;

            case "W":
                Instantiate(myEnemyUnitArray[1], transform.position, transform.rotation);
                break;

            case "E":
                Instantiate(myEnemyUnitArray[2], transform.position, transform.rotation);
                break;

            case "R":
                Instantiate(myEnemyUnitArray[3], transform.position, transform.rotation);
                break;

            case "T":
                Instantiate(myEnemyUnitArray[4], transform.position, transform.rotation);
                break;
        }
    }



    //엔드포인트 도달시 라운드가 마지막이 아니라면 다음라운드 , 마지막이면 승리
    void ClearRound()
    {

    }



}
