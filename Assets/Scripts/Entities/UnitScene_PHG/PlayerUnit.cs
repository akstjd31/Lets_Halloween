using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private AudioClip attackAudio;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private float attackDelay;
    private float attackTime;

    private bool isPlayerUnit; // 진영에따른 플레이어유닛 / 적군유닛 구분

    private bool isAttack;

    private int lastAttackIndex;    //마지막으로 공격한 모션 인덱스

    private GameObject target;

    Weapon weapon;  //원거리타입 무기 가져옴
    Weapon[] weapons; //소환체의 무기들 가져옴

    //시작시 애니메이터 , 오디오 소스 가져옴
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (gameObject.tag== "PlayerUnit_Projectile")
        {
            weapons = GetComponentsInChildren<Weapon>();
        }

        else if(gameObject.tag=="PlayerUnit")
        {
            weapon = GetComponentInChildren<Weapon>();
        }
           
    }

    //충돌 발생
    private void OnTriggerEnter(Collider other)
    {
        //Collider thisObjectCollider = gameObject.GetComponent<Collider>();
        //Collider otherObjectCollider = other.GetComponent<Collider>();

        ////충돌한 유닛의 태그가 Player를 포함하고 있을경우 충돌무시
        //if (other.tag.Contains("Player"))
        //{
        //    Physics.IgnoreCollision(thisObjectCollider, otherObjectCollider);   //트리거 충돌 무시
        //}
        if (other.tag == "EnemyUnit")
        {
            Debug.Log("적군 진입");
            isAttack = true;
            transform.LookAt(other.transform);  //타겟을 바라봄
            attackTime = attackDelay;   //적군 바로공격할수있게 쿨타임 충전
            target = other.gameObject;
        }
       
    }

    

    private void Update()
    {
        if (isAttack && target != null)  // 현재 공격 대상이 존재할 때만 회전
        {
            transform.LookAt(target.transform);
        }

        //유닛이면 어택 , 설치류면 Shoot
        if (gameObject.tag == "PlayerUnit")
        {
            Attack();
        }
        else if (gameObject.tag == "PlayerUnit_Projectile")
        {
            foreach (var muzzle in weapons) { muzzle.Shoot(target.transform); }
        }
    }

    // 적이 범위를 벗어나면 정지상태로 돌입
    private void OnTriggerExit(Collider other)
    {
        isAttack = false;
    }

    void Attack()
    {
        if (isAttack == true && target != null)   //적이 범위안에있음
        {
            attackTime += Time.deltaTime;

            if (attackTime >= attackDelay)
            {
              
                //현재 애니메이션 상태 가져옴
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

                if (!state.IsTag("Attack"))
                {
                    int randomActionIndex;    //행동할 랜덤인덱스 
                    do
                    {
                        if (weapon != null)    //무기 컴포넌트가 없으면 근거리 유닛 . 모션 0~3
                        {
                            randomActionIndex = Random.Range(0, 2);
                        }
                        else
                        {
                            randomActionIndex = Random.Range(0, 4);
                        }

                    } while (randomActionIndex == lastAttackIndex);

                    lastAttackIndex = randomActionIndex;

                    animator.SetTrigger($"Attack{randomActionIndex}");

                }
                attackTime = 0;

            }
        }
    }
  

    //유니티 애니메이션 함수
    void Shoot()
    {
        weapon.Shoot(target.transform);
    }

    //효과음 재생
    void AttackSound()
    {
        audioSource.Play();
    }


}
