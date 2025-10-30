using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private Vector3 startPosition;

    [SerializeField] GameObject roundClearText; //임시

    //몹 스폰 딜레이
    [SerializeField] private float spawnDelay;
    private float spawnTime = 0;

    private void Start()
    {
        transform.position = GameObject.FindWithTag("StartPoint").transform.position;
        wayPointBox = GameObject.Find("WayPoint").transform;
        roundClearText.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = GameObject.FindWithTag("StartPoint").transform.position;
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
            ClearRound();
            gameObject.SetActive(false);
            
        }
    }

    private void Update()
    {
        spawnTime += Time.deltaTime;
        MoveObj();
        CreateEnemyUnit();
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
    //몹소환
    private void CreateEnemyUnit()
    {
        switch (Input.inputString.ToUpper())
        {
            case "Q":
                SpawnUnit(0, currentWayPointIndex);
                break;

            case "W":
                SpawnUnit(1, currentWayPointIndex);
                break;

            case "E":
                SpawnUnit(2, currentWayPointIndex);
                break;

            case "R":
                SpawnUnit(3, currentWayPointIndex);
                break;

            case "T":
                SpawnUnit(4, currentWayPointIndex);
                break;
        }
    }
    //키입력에 따른 몬스터 소환
    private void SpawnUnit(int mobIndex, int WayPointIndex)
    {
        if (spawnTime >= spawnDelay)
        {
            GameObject mob = Instantiate(myEnemyUnitArray[mobIndex], transform.position, transform.rotation);
            mob.GetComponent<EnemyUnit>().SetWayPoint(currentWayPointIndex);
            spawnTime = 0;
        }
    }

    //엔드포인트 도달시 라운드가 마지막이 아니라면 다음라운드 , 마지막이면 승리
    void ClearRound()
    {
        roundClearText.SetActive(true);
    }
}
