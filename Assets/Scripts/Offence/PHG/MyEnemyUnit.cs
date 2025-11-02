using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyEnemyUnit : MonoBehaviour
{
    //?????????
    [SerializeField] private float moveSpeed;

    public float MoveSpeed => moveSpeed;

    [SerializeField] private float rotateSpeed;
    private int currentWayPointIndex = 0;

    private Transform wayPointBox;
    [SerializeField] private int hp;
    [SerializeField] private string EnemyName;

    //???????? ?¡À???? ??“N????
    [SerializeField] private GameObject[] myEnemyUnitArray;

    private Vector3 startPosition;

    [SerializeField] GameObject roundClearText; //???

    //?? ???? ??????
    [SerializeField] private float spawnDelay;
    private float spawnTime = 0;


    private void Start()
    {
        transform.position = GameObject.FindWithTag("StartPoint").transform.position;
        wayPointBox = GameObject.Find("WayPoint").transform;
        //roundClearText.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = GameObject.FindWithTag("StartPoint").transform.position;
    }

    //????????? ?úô???
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
            Debug.Log($"{gameObject.name} ?????? ????");
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

    //??????? ???
    private void MoveObj()
    {
        if (wayPointBox == null || currentWayPointIndex >= wayPointBox.childCount) { return; }

        Transform targetTransform = wayPointBox.GetChild(currentWayPointIndex);
        Vector3 direction = (targetTransform.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position,targetTransform.position,moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed *Time.deltaTime);

        if (direction == Vector3.zero)
        {
            currentWayPointIndex++;
        }
    }
    //?????
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
                spawnTime -= 1; //???? ????? 1?? ?????? ????
                break;
        }
    }
    //???¢¯? ???? ???? ???
    private void SpawnUnit(int mobIndex, int WayPointIndex)
    {
        if (spawnTime >= spawnDelay)
        {
            GameObject mob = Instantiate(myEnemyUnitArray[mobIndex], transform.position, transform.rotation);
            mob.GetComponent<EnemyUnit>().SetWayPoint(currentWayPointIndex);
            spawnTime = 0;
        }
    }

    //????????? ????? ???? ???????? ????? ???????? , ????????? ?¢¬?
    void ClearRound()
    {
        //roundClearText.SetActive(true);
    }
}
