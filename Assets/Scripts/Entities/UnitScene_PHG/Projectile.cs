using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    //투사체 속도
    [SerializeField] private float projectilemoveSpeed = 10;
    private Transform targetTransform;

    private int power;

    public int Power => power;

    WaitForSeconds activeStatus;


    void Update()
    {
        ActivateAction();
    }

    //투사체는 타겟에게 날아감
    private void ActivateAction()
    {
        if (targetTransform == null) { return; }

        transform.rotation = Quaternion.LookRotation(targetTransform.position);
        transform.position = Vector3.MoveTowards(transform.position,
            targetTransform.position,
            projectilemoveSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    //적 유닛한테 맞으면 비활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Enemy"))
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();

            if(enemy!= null)
            {
                enemy.TakeDamage(power);
                gameObject.SetActive(false);
            }
        }

        else if(other.tag =="EndPoint")
        {
            Debug.Log("투사체 앤드포인트 도달 , 사라짐");
            gameObject.SetActive(false);
        }
    }

    //투사체 공격력지정
    public void SetPower(int _power)
    {
        power = _power;
    }

    //랜덤 상태이상 지정
    void StatusEffect()
    {

    }

}
