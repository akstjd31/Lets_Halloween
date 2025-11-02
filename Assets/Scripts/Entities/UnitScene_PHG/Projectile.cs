using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Projectile : MonoBehaviour 
{
    //투사체 속도
    [SerializeField] private float projectilemoveSpeed = 10;
    private Transform targetTransform;

    private int power;

    private PlayerUnit playerUnit;


    void Update()
    {
        ActivateAction();

        if (!targetTransform.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
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
      
    //적 유닛한테 맞으면 비활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Enemy"))
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();

            MyEnemyUnit myEnemyUnit = other.GetComponent<MyEnemyUnit>();

            if(enemy!= null)
            {
                enemy.TakeDamage(power);

                if(playerUnit !=null)
                {
                    float randomPercent = Random.value; // 0.0 ~ 1.0 사이

                    if (randomPercent <= playerUnit.PassiveSkillPercent)
                    {
                        playerUnit.ApplyStatusEffect(playerUnit.PassiveSkill);  //패시브스킬 발동
                    }

                }

                gameObject.SetActive(false);
            }

            if(myEnemyUnit != null)
            {
                if (playerUnit != null)
                {
                    float randomPercent = Random.value; // 0.0 ~ 1.0 사이

                    if (randomPercent <= playerUnit.PassiveSkillPercent)
                    {
                        playerUnit.ApplyStatusEffect(playerUnit.PassiveSkill);  //패시브스킬 발동
                    }

                }

                gameObject.SetActive(false);
            }

            else
            {
                gameObject.SetActive(false);
            }
        }

        else if(other.tag =="EndPoint")
        {
            Debug.Log("투사체 앤드포인트 도달 , 사라짐");
            gameObject.SetActive(false);
        }

        if (!other.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

    }

    //투사체 공격력지정
    public void SetPower(int _power)
    {
        power = _power;
    }
    //타겟 지정
    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
    public void SetPlayerUnit (PlayerUnit _playerUnit)
    {
        playerUnit = _playerUnit;
    }

}
