using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //투사체 속도
    [SerializeField] private float projectilemoveSpeed = 10;

    private Transform targetTransform;

    void Update()
    {
        ActivateAction();
    }

    //투사체는 타겟에게 날아감
    private void ActivateAction()
    {
        if (targetTransform == null) { return; }

        transform.position = Vector3.MoveTowards(transform.position,
            targetTransform.position,
            projectilemoveSpeed * Time.deltaTime);

    }

    //적 유닛한테 맞으면 비활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyUnit")
        {
            Debug.Log("적과 충돌");
            gameObject.SetActive(false);
        }
    }
    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
}
