using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class PlayerUnit : MonoBehaviour , IUnitPassiveSkill
{
    [SerializeField] private AudioClip attackAudio;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private int power;
    [SerializeField] private string name;
    [SerializeField] private string passiveName;
    [SerializeField] private string passiveEffect;
    [SerializeField] private int price;
    public int Power => power;
    public string Name => name;
    public string PassiveName => passiveName;
    public string PassiveEffect => passiveEffect;
    public int Price => price;

    [SerializeField] private float attackDelay;

    public float AttackDelay => attackDelay;
    private float attackTime;

    private bool isPlayerUnit; // 진영에따른 플레이어유닛 / 적군유닛 구분

    private bool isAttack;

    private int lastAttackIndex;    //마지막으로 공격한 모션 인덱스

    private GameObject target;

    Weapon weapon;  //원거리타입 무기 가져옴

    EnemyUnit enemy;    //데미지를 줄 적유닛.
    MyEnemyUnit myEnemy; //상태이상을 걸 플레이어 유닛

    Renderer render;

    //유닛패시브스킬
    [Header("PassiveSkill")]
    [SerializeField] PassiveSkill passiveSkill; 
    [SerializeField] float passiveSkillPercent;
    private WaitForSeconds passiveSkillDuration;
    [SerializeField] private float passiveSkillTime;
    [SerializeField] private GameObject passiveSkillEffect;
    private static bool isStatus;
    private float statusEffectDelay = 0;    //상태이상 다시 걸기전까지의 딜레이

    public PassiveSkill PassiveSkill => passiveSkill;
    public float PassiveSkillPercent => passiveSkillPercent;

    //시작시 애니메이터 , 오디오 소스 가져옴
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<Weapon>();
        passiveSkillDuration = new WaitForSeconds(passiveSkillTime);
    }

    //충돌 발생
    //구분 이유 -> 타겟우선 -> Enemy
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag ==("Enemy"))
        {
            Debug.Log("적군 진입");
            isAttack = true;
            transform.LookAt(other.transform);  //타겟을 바라봄
            attackTime = attackDelay;   //적군 바로공격할수있게 쿨타임 충전
            target = other.gameObject;
            enemy = other.GetComponent<EnemyUnit>();
            
        }
        else if(other.tag== ("EnemyUnit"))
        {
            Debug.Log("적플레이어 진입");
            isAttack = true;
            transform.LookAt(other.transform);  //타겟을 바라봄
            attackTime = attackDelay;   //적군 바로공격할수있게 쿨타임 충전
            target = other.gameObject;
            myEnemy = other.GetComponent<MyEnemyUnit>();
        }
    }

    private void Update()
    {
        if (isAttack && target.activeSelf)  // 현재 공격 대상이 존재할 때만 회전
        {
            transform.LookAt(target.transform);
            Attack();   
        }
    }

    // 적이 범위를 벗어나면 정지상태로 돌입
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Enemy"))
        {
            isAttack = false;
        }    
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

    public void ApplyStatusEffect(PassiveSkill type)
    {
        //적군유닛
        if (enemy != null)
        {
            //상태이상별 기능 
            switch (type)
            {
                case PassiveSkill.Slow:  //아처에 추가할것
                    StartCoroutine(Slow(enemy));
                    break;

                case PassiveSkill.Stun:  //마법사에 추가
                    StartCoroutine(Stun(enemy));
                    break;

                case PassiveSkill.DoubleAttack:  //바바리안에 추가할것임
                    StartCoroutine(DoubleAttack(enemy, power));
                    break;
            }
        }

        //플레이어 적유닛
        if (myEnemy != null)
        {
            //상태이상별 기능 
            switch (type)
            {
                case PassiveSkill.Slow:  //아처에 추가할것
                    StartCoroutine(Slow(myEnemy));
                    break;

                case PassiveSkill.Stun:  //마법사에 추가
                    StartCoroutine(Stun(myEnemy));
                    break;
            }
        }

    }

 
    #region 플레이어 패시브 스킬

    //더블어택. 두번공격함
    private IEnumerator DoubleAttack(EnemyUnit target , int Damage)
    {
        if (target == null || !target.gameObject.activeSelf) yield break;

        Debug.Log("더블어택 발동");
        target.TakeDamage(Damage);

        GameObject obj = Instantiate(passiveSkillEffect,target.transform.position,target.transform.rotation);

        yield return passiveSkillDuration;

        Destroy(obj);

    }

    private IEnumerator Stun(EnemyUnit target) 
    {
        if (target == null || isStatus || !target.gameObject.activeSelf || target.MoveSpeed==0) yield break;

        GameObject obj;
      
        Debug.Log("스턴 발동");
        isStatus = true;
        obj = Instantiate(passiveSkillEffect, target.transform.position, target.transform.rotation);
        obj.transform.SetParent(target.transform);
        target.StatusEffectColor(PassiveSkill); //유닛 마태리얼 색상변경

        target.MoveSpeed = 0;

        yield return passiveSkillDuration;

        target.MoveSpeed = target.moveSpeedOrigin;
        Destroy(obj);
        Debug.Log("스턴 풀림");
        target.ReturnStatusEffectColor();
        isStatus = false;
    }

    //스턴오버로딩
    private IEnumerator Stun(MyEnemyUnit target)
    {
        if (target == null || isStatus || !target.gameObject.activeSelf || target.MoveSpeed == 0) yield break;

        GameObject obj;
        float originMoveSpeed = target.MoveSpeed;

        Debug.Log("스턴 발동");
        isStatus = true;
        obj = Instantiate(passiveSkillEffect, target.transform.position, target.transform.rotation);
        obj.transform.SetParent(target.transform);
        target.StatusEffectColor(PassiveSkill); //유닛 마태리얼 색상변경

        target.MoveSpeed = 0;

        yield return passiveSkillDuration;

        target.MoveSpeed = originMoveSpeed;
        Destroy(obj);
        Debug.Log("스턴 풀림");
        target.ReturnStatusEffectColor();
        isStatus = false;
    }

    private IEnumerator Slow(EnemyUnit target)
    {
        if (target == null || isStatus || !target.gameObject.activeSelf ) yield break;

        float originMoveSpeed = target.MoveSpeed;
        GameObject obj;

        if (originMoveSpeed < target.MoveSpeed) yield break;

        Debug.Log("슬로우 발동");
        isStatus = true;
        obj = Instantiate(passiveSkillEffect, target.transform.position, target.transform.rotation);
        target.MoveSpeed = target.MoveSpeed / 2;

        target.StatusEffectColor(PassiveSkill); //유닛 마태리얼 색상변경


        yield return passiveSkillDuration;

        target.MoveSpeed = originMoveSpeed;
        Destroy(obj);
        Debug.Log("슬로우 풀림");
        target.ReturnStatusEffectColor();
        isStatus = false;
    }

    //오버로딩
    private IEnumerator Slow(MyEnemyUnit target)
    {
        if (target == null || isStatus || !target.gameObject.activeSelf) yield break;

        float originMoveSpeed = target.MoveSpeed;
        GameObject obj;

        if (originMoveSpeed < target.MoveSpeed) yield break;

        Debug.Log("슬로우 발동");
        isStatus = true;
        obj = Instantiate(passiveSkillEffect, target.transform.position, target.transform.rotation);
        target.MoveSpeed = target.MoveSpeed / 2;

        target.StatusEffectColor(PassiveSkill); //유닛 마태리얼 색상변경

        yield return passiveSkillDuration;

        target.MoveSpeed = originMoveSpeed;
        Destroy(obj);
        Debug.Log("슬로우 풀림");
        target.ReturnStatusEffectColor();
        isStatus = false;
    }

    #endregion

    #region 유니티 애니메이션 함수

    //유니티 애니메이션 함수
    void Shoot()
    {
        weapon.Shoot(target.transform);
    }

    void TakeDamage()
    {
        if (enemy != null)
        {
            float randomPercent = Random.Range(0f, 1f);

            enemy.TakeDamage(power);

            //유닛 패시브 발동
            if(randomPercent<=passiveSkillPercent)
            {
                ApplyStatusEffect(passiveSkill);
            }
        }

        if(myEnemy != null)
        {
            float randomPercent = Random.Range(0f, 1f);

            //유닛 패시브 발동
            if (randomPercent <= passiveSkillPercent)
            {
                ApplyStatusEffect(passiveSkill);
            }

        }
    }

    //효과음 재생
    void AttackSound()
    {
        audioSource.Play();
    }
    #endregion

}