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

    private int lastAttackIndex;    //마지막으로 공격한 모션 인덱스

    private GameObject target;

    [SerializeField] private int power;

    public int Power => power;

    Weapon[] weapons; //소환체의 무기들 가져옴

    //시작시 애니메이터 , 오디오 소스 가져옴
    private void Start()
    {
        weapons = GetComponentsInChildren<Weapon>();         
    }

    //충돌 발생
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains ("Enemy"))
        {
            Debug.Log("적군 진입");
            shootTime = shootDelay;   //적군 바로공격할수있게 쿨타임 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.activeSelf && other.tag.Contains("Enemy"))
        {
            transform.LookAt(other.transform);
            target = other.gameObject;
            Attack();
        }
    }

    void Attack()
    {
        shootTime += Time.deltaTime;

        if (shootTime >= shootDelay)
        {
            foreach (var muzzle in weapons) { muzzle.Shoot(target.transform); }
        }
        
    }
  
    //효과음 재생
    void AttackSound()
    {
        audioSource.Play();
    }


}
