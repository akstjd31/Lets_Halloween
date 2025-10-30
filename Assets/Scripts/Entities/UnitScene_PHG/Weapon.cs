using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //투사체 넣을 프리팹
    [SerializeField] private GameObject projectilePrefab;
    //투사체 풀 사이즈
    [SerializeField] private int projectilePoolSize;

    private int projectileCount;
    GameObject[] projectilePrefabs;

    Transform targetTransform;

    Projectile projectile; //투사체

    PlayerUnit playerUnit;

    PlayerUnit_Projectile playerUnit_Projectile;


    private void Start()
    {
        Init();
    }

    void Init()
    {
        bool isProjectile = false;  //플레이어 소환체유닛인지 여부확인

        projectilePrefabs = new GameObject[projectilePoolSize];
        projectileCount = projectilePoolSize;   //풀 충당

        //플레이어 또는 플레이어소환체 유닛컴포넌트 가져와서 공격력 할당
        playerUnit = transform.root.GetComponent<PlayerUnit>();

        if(playerUnit == null)
        {
            playerUnit_Projectile = transform.root.GetComponent<PlayerUnit_Projectile>();
            isProjectile = true;
        }

        for (int i=0; i<projectilePrefabs.Length; i++)
        {
            projectilePrefabs[i]=Instantiate(projectilePrefab,transform.position , Quaternion.identity);
            projectilePrefabs[i].SetActive(false);  //비활성화
            projectile = projectilePrefabs[i].GetComponent<Projectile>();

            if (isProjectile)
            {
                projectile.SetPower(playerUnit_Projectile.Power);
            }
            else 
            {
                projectile.SetPower(playerUnit.Power);
            }
        }
    }

    public void Shoot(Transform target)
    {
        foreach (var obj in projectilePrefabs) 
        {
            //현재 활성화되어있는지 확인후 비활이면 활성화
            if(!obj.activeSelf)
            {
                obj.transform.position = transform.position; 
                obj.SetActive(true);
                obj.GetComponent<Projectile>().SetTarget(target);
                return;
            }
        }
    }
}
