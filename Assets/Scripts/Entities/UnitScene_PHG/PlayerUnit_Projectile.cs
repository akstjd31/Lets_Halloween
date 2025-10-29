using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class PlayerUnit_Projectile : MonoBehaviour
{
    [SerializeField] private AudioClip attackAudio;
    private AudioSource audioSource;

    [SerializeField] private float shootDelay;
    private float shootTime;

    private bool isPlayerUnit; // 진영에따른 플레이어유닛 / 적군유닛 구분

    private bool isAttack;

    private int lastAttackIndex;    //마지막으로 공격한 모션 인덱스

    private GameObject target;

    Weapon[] weapons; //소환체의 무기들 가져옴

    //시작시 애니메이터 , 오디오 소스 가져옴
    private void Start()
    {
        weapons = GetComponentsInChildren<Weapon>();         
    }

    //충돌 발생
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyUnit")
        {
            Debug.Log("적군 진입");
            isAttack = true;
            transform.LookAt(other.transform);  //타겟을 바라봄
            shootTime = shootDelay;   //적군 바로공격할수있게 쿨타임 충전
            target = other.gameObject;
        }
    }


    private void Update()
    {
        if (isAttack && target != null)  // 현재 공격 대상이 존재할 때만 회전
        {
            transform.LookAt(target.transform);
            Attack();
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
            shootTime += Time.deltaTime;

            if (shootTime >= shootDelay)
            {
                foreach (var muzzle in weapons) { muzzle.Shoot(target.transform); }
            }
        }
    }
  
    //효과음 재생
    void AttackSound()
    {
        audioSource.Play();
    }


}
