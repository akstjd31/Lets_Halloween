using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyUnit : MonoBehaviour
{
    //프로토타입
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    private int currentWayPointIndex = 0;

    [SerializeField] private Transform wayPointBox;

    //웨이포인트 충돌처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("WayPoint"))
        {
            if (wayPointBox == null) { return; }

            currentWayPointIndex++;

            if (currentWayPointIndex >= wayPointBox.childCount) { currentWayPointIndex = 0; }
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
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed *Time.deltaTime);

        if(direction == Vector3.zero)
        {
            currentWayPointIndex++;
        }
    }






}
